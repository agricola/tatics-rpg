using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapPosition
{
    [SerializeField]
    private int x, y;
    public int X { get { return x; } }
    public int Y { get { return y; } }
    public MapPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public override string ToString()
    {
        return "(" + x + ", " + y + ")";
    }
    public static MapPosition VectorToMapPosition(Vector2 vector)
    {
        return new MapPosition((int)vector.x, (int)vector.y);
    }

    public MapPosition Increment(bool horizontal, int direction)
    {
        if (horizontal)
        {
            return new MapPosition(x + direction, y);
        }
        else
        {
            return new MapPosition(x, y + direction);
        }
    }

}
