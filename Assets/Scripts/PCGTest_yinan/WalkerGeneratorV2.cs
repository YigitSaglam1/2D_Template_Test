using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Dropping2D;
public class WalkerGeneratorV2 : MonoBehaviour
{
    public enum Grid
    {
        FLOOR,
        SEA,
        EMPTY,
        MOUNTAIN
    }


    public int MapWeight = 100;
    public int MapHeight = 200;

    Grid[,] allGrids;
    public Tilemap tileMap;
    public Tile Floor;
    public Tile Sea;
    public Tile Mountain;
    public float WaitTime = 0.005f;
    List<WalkerObject> Walkers;

    private void Start()
    {
        Initialize();

    }
    public void Initialize()
    {
        allGrids = new Grid[MapWeight, MapHeight];
        for (int x = 0; x < allGrids.GetLength(0); x++)
        {
            for (int y = 0; y < allGrids.GetLength(1); y++)
            {
                allGrids[x, y] = Grid.EMPTY;
            }
        }

        Vector3Int startPos = new Vector3Int(1,1,0);
        Walkers = new List<WalkerObject>();
        WalkerObject walkerObject = new WalkerObject(new Vector2(startPos.x, startPos.y), Vector2.up, new Vector2(startPos.x, startPos.y ));
        allGrids[startPos.x, startPos.y] = Grid.FLOOR;
        tileMap.SetTile(startPos, Floor);
        Walkers.Add(walkerObject);
        UpdatePosition(1);
        StartCoroutine(TileGeneration());
    }
    private IEnumerator TileGeneration()
    {
        for (int i = 0; i < Mathf.CeilToInt(allGrids.GetLength(0)); i++)
        {
            while (Walkers[0].Position != Walkers[0].birthPosition)
            {

                Vector3Int curPos = new Vector3Int((int)Walkers[0].Position.x, (int)Walkers[0].Position.y, 0);
                bool hasCreatedFloor = false;
                if (allGrids[curPos.x, curPos.y] != Grid.FLOOR)
                {
                    tileMap.SetTile(curPos, Floor);

                    allGrids[curPos.x, curPos.y] = Grid.FLOOR;
                    hasCreatedFloor = true;
                }
                UpdatePosition(i);
                // Go to right
                if (Walkers[0].Position.y == allGrids.GetLength(1) - 2)
                {
                    Walkers[0].Direction = Vector2.right;
                }
                // Go to down
                if (Walkers[0].Position.x == allGrids.GetLength(0) - 2)
                {
                    Walkers[0].Direction = Vector2.down;
                }
                if (Walkers[0].Position.y == 1)
                {
                    Walkers[0].Direction = Vector2.left;
                }


                if (hasCreatedFloor)
                {
                    yield return new WaitForSeconds(WaitTime);
                }
            }

            Walkers.RemoveAt(0);
            Vector3Int startPos = new Vector3Int(i, i, 0);
            WalkerObject walkerObject = new WalkerObject(new Vector2(startPos.x, startPos.y), Vector2.up, new Vector2(startPos.x, startPos.y));
            allGrids[startPos.x, startPos.y] = Grid.FLOOR;
            tileMap.SetTile(startPos, Floor);
            Walkers.Add(walkerObject);
            UpdatePosition(i);
        }

    }


    private void UpdatePosition(int offset)
    {
        for (int i = 0; i < Walkers.Count; i++)
        {
            WalkerObject FoundWalker = Walkers[i];
            FoundWalker.Position += FoundWalker.Direction;
            FoundWalker.Position.x = Mathf.Clamp(FoundWalker.Position.x, offset, allGrids.GetLength(0) - 1 - offset);
            FoundWalker.Position.y = Mathf.Clamp(FoundWalker.Position.y, offset, allGrids.GetLength(1) - 1 - offset);
            Walkers[i] = FoundWalker;
        }
    }
}
