using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPosition
{
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

}
