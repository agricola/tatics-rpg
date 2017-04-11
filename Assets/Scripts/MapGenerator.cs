using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    [SerializeField]
    private List<MapTiles> LevelMaps = new List<MapTiles>() { };
    private static readonly Dictionary<char, TileType> tileDictionary
        = new Dictionary<char, TileType>()
        {
            { '#', TileType.Wall},
            { '.', TileType.Ground},
            { '1', TileType.GoodSpawn },
            { '2', TileType.BadSpawn }
        };

    // Use this for initialization
    void Start () {
        GenerateMap();
        EventManager.Instance.Raise(new FinishGeneratingMapsEvent(LevelMaps));
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void GenerateRandomMap()
    {

    }

    private void GenerateMap()
    {
        string fileName = "./Assets/Maps/map1.txt";
        MapTiles mapTiles = MapReader<MapTiles, TileType>.ReadMapFile(fileName, ListToMapTiles, tileDictionary);
        LevelMaps.Add(mapTiles);
    }

    private MapTiles ListToMapTiles(List<List<TileType>> rows)
    {
        TileRow[] tiles = new TileRow[rows.Count];
        for (int i = 0; i < rows.Count; i++)
        {
            tiles[i] = new TileRow(rows[i].ToArray());
        }
        TileRow[] rev = tiles.Reverse().ToArray();
        return new MapTiles(rev);
    }
}
