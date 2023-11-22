using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WalkerObjectGenerator : MonoBehaviour
{
    public enum Grid
    {
        FLOOR,
        WALL,
        EMPTY
    }

    public Grid[,] gridHandler;
    public List<WalkerObject> Walkers;
    public Tilemap tileMap;
    public List<Tile> floors;
    public List<Tile> walls;
    public int MapWidth = 30;
    public int MapHeight = 30;
    public int maxWalkers = 10;
    public int TileCount = default;
    public float FillPercentage = 0.4f;
    public float WaitTime = 0.05f;

    [Header("Tree Options")]
    public GameObject tree;

    private void Start()
    {
        InitializeGrid();
    }

    public void InitializeGrid()
    {
        gridHandler = new Grid[MapWidth, MapHeight];

        for (int x = 0; x < gridHandler.GetLength(0); x++)
        {
            for (int y = 0; y < gridHandler.GetLength(1); y++)
            {
                gridHandler[x, y] = Grid.EMPTY;
            }
        }

        Walkers = new List<WalkerObject>();

        Vector3Int TileCenter = new Vector3Int(gridHandler.GetLength(0) / 2, gridHandler.GetLength(1) / 2, 0);

        WalkerObject curWalker = new WalkerObject(new Vector2(TileCenter.x, TileCenter.y), GetDirection(), 0.5f);
        gridHandler[TileCenter.x, TileCenter.y] = Grid.FLOOR;
        SetTile(TileCenter, floors);
        Walkers.Add(curWalker);

        TileCount++;

        StartCoroutine(CreateFloor());
        
    }

    private IEnumerator CreateObject()
    {
        for (int x = 0; x < gridHandler.GetLength(0); x++)
        {
            for (int y = 0; y < gridHandler.GetLength(1); y++)
            {
                if (gridHandler[x, y] == Grid.FLOOR)
                {
                    
                    if (UnityEngine.Random.Range(0, 20) == 1)
                    {
                        Instantiate(tree, new Vector3(x, y, 0), Quaternion.identity);
                        yield return new WaitForSeconds(WaitTime);
                    }
                    
                }
            }
        }
    }

    //Randomly sets the tiles from list
    private void SetTile(Vector3Int TileCenter, List<Tile> tileList)
    {
        int index = UnityEngine.Random.Range(0, tileList.Count);
        tileMap.SetTile(TileCenter, tileList[index]);
    }

    Vector2 GetDirection()
    {
        int choice = Mathf.FloorToInt(UnityEngine.Random.value * 3.99f); // Try (random int between 0,4)

        switch (choice)
        {
            case 0:
                return Vector2.down;
            case 1: 
                return Vector2.left;
            case 2:
                return Vector2.up;
            case 3:
                return Vector2.right;
            default: 
                return Vector2.zero;
        }
    }

    private IEnumerator CreateFloor()
    {
        while ((float)TileCount / (float)gridHandler.Length < FillPercentage)
        {
            bool hasCreatedFloor = false;
            foreach (WalkerObject curWalker in Walkers) 
            {
                Vector3Int curPos = new Vector3Int((int)curWalker.Position.x, (int)curWalker.Position.y, 0);

                if (gridHandler[curPos.x,curPos.y] != Grid.FLOOR)
                {
                    SetTile(curPos, floors);
                    TileCount++;
                    gridHandler[curPos.x, curPos.y] = Grid.FLOOR;
                    hasCreatedFloor = true;
                }
            }

            ChanceToRemove();
            ChanceToRedirect();
            ChanceToCreate();
            UpdatePosition();

            if (hasCreatedFloor)
            {
                yield return new WaitForSeconds(WaitTime);
            }           

        }
        StartCoroutine(CreateWalls());
        StartCoroutine(CreateObject());
    }
    private IEnumerator CreateWalls()
    {
        for (int x = 0; x < gridHandler.GetLength(0) -1 ; x++)
        {
            for (int y = 0; y < gridHandler.GetLength(1) -1 ; y++)
            {
                // because of the if statement and clamping the floor grid, there can not be ArrayIndexOutOfBounds error.
                if (gridHandler[x,y] == Grid.FLOOR)
                {
                    bool hasCreatedWall = false;
                    if (gridHandler[x + 1, y] == Grid.EMPTY)
                    {
                        SetTile(new Vector3Int(x + 1, y, 0), walls);
                        gridHandler[x + 1, y] = Grid.WALL;
                        hasCreatedWall = true;
                    }
                    if (gridHandler[x - 1, y] == Grid.EMPTY)
                    {
                        SetTile(new Vector3Int(x - 1, y, 0), walls);
                        gridHandler[x - 1, y] = Grid.WALL;
                        hasCreatedWall = true;
                    }
                    if (gridHandler[x , y + 1] == Grid.EMPTY)
                    {
                        SetTile(new Vector3Int(x, y + 1, 0), walls);
                        gridHandler[x, y + 1] = Grid.WALL;
                        hasCreatedWall = true;
                    }
                    if (gridHandler[x, y - 1] == Grid.EMPTY)
                    {
                        SetTile(new Vector3Int(x, y - 1, 0), walls);
                        gridHandler[x, y - 1] = Grid.WALL;
                        hasCreatedWall = true;
                    }

                    if (hasCreatedWall)
                    {
                        yield return new WaitForSeconds(WaitTime);
                    }
                }
            }
        }
    }
    private void ChanceToRemove()
    {
        int updatedCount = Walkers.Count;
        for (int i = 0; i < updatedCount; i++)
        {
            if (UnityEngine.Random.value < Walkers[i].ChanceToChange && Walkers.Count > 1)
            {
                Walkers.RemoveAt(i);
                break;
            }
        }
    }
    private void ChanceToRedirect()
    {
        for (int i = 0; i < Walkers.Count; i++)
        {
            if (UnityEngine.Random.value < Walkers[i].ChanceToChange)
            {
                WalkerObject curWalker = Walkers[i];
                curWalker.Direction = GetDirection();
                Walkers[i] = curWalker;
            }
        }
    }
    private void ChanceToCreate()
    {
        int updatedCount = Walkers.Count;
        for (int i = 0; i < updatedCount; i++)
        {
            if (UnityEngine.Random.value < Walkers[i].ChanceToChange && Walkers.Count < maxWalkers)
            {
                Vector2 newDirection = GetDirection();
                Vector2 newPosition = Walkers[i].Position;

                WalkerObject newWalker = new WalkerObject(newPosition, newDirection, 0.5f);
                Walkers.Add(newWalker);
            }
        }
    }
    private void UpdatePosition()
    {
        for (int i = 0; i < Walkers.Count; i++)
        {
            WalkerObject FoundWalker = Walkers[i];
            FoundWalker.Position += FoundWalker.Direction;
            FoundWalker.Position.x = Mathf.Clamp(FoundWalker.Position.x, 1, gridHandler.GetLength(0) - 2);
            FoundWalker.Position.y = Mathf.Clamp(FoundWalker.Position.y, 1, gridHandler.GetLength(1) - 2);
            Walkers[i] = FoundWalker;
        }
    }
}
