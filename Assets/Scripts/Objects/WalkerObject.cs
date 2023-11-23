using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerObject
{
    public Vector2 Position;
    public Vector2 Direction;
    public float ChanceToChange; //WTF is that
    public readonly Vector2 birthPosition;
    public WalkerObject(Vector2 position, Vector2 direction, float chanceToChange)
    {
        Position = position;
        Direction = direction;
        ChanceToChange = chanceToChange;
    }
    public WalkerObject(Vector2 position, Vector2 direction, Vector2 birthPosition)
    {
        this.Position = position;
        this.Direction = direction;
        this.birthPosition = birthPosition;
    }
}
