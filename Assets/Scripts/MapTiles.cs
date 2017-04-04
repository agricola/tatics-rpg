using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public enum TileType { Ground, Wall, GoodSpawn, BadSpawn }

[System.Serializable]
public class TileRow
{
    [SerializeField]
    private TileType[] row;
    public TileType[] Row
    {
        get
        {
            return row;
        }
    }
    public TileRow(TileType[] row)
    {
        this.row = row;
    }
}

[System.Serializable]
public class MapTiles
{
    [SerializeField]
    private TileRow[] tiles;
    public TileRow[] Tiles
    {
        get
        {
            return tiles;
        }
    }
    public MapTiles(TileRow[] tiles)
    {
        this.tiles = tiles;
    }
}
