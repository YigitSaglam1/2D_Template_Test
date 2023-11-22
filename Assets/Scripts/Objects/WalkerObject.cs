using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerObject
{
    public Vector2 Position;
    public Vector2 Direction;
    public float ChanceToChange; //WTF is that

    public WalkerObject(Vector2 position, Vector2 direction, float chanceToChange)
    {
        Position = position;
        Direction = direction;
        ChanceToChange = chanceToChange;
    }   
}
