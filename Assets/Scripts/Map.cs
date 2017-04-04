using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField]
    private Tile templateTile;
    [SerializeField]
    private Tile templateBlockedTile;
    [SerializeField]
    private int width = 0;
    [SerializeField]
    private int height = 0;
    [SerializeField]
    private Tile[,] tiles = new Tile[0,0];
    [SerializeField]
    private GameObject baseCharacter;
    [SerializeField]
    private float openTilePercentage = 0.9f;
    [SerializeField]
    private List<MapPosition> goodSpawn = new List<MapPosition>();
    [SerializeField]
    private List<MapPosition> badSpawn = new List<MapPosition>();

    public int Width
    {
        get
        {
            return width;
        }
    }
    public int Height
    {
        get
        {
            return height;
        }
    }
    public Tile[,] Tiles
    {
        get
        {
            return tiles;
        }
    }

    public bool IsWithinBounds(int x, int y)
    {
        return (x < Width && y < Height && x >= 0 && y >= 0) ? true : false;
    }

    public List<Tile> GetNeighbors(MapPosition pos)
    {
        return GetNeighbors(pos.X, pos.Y);
    }

    public List<Tile> GetNeighbors(int x, int y)
    {
        List<Tile> neighbors = new List<Tile>();
        if (IsWithinBounds(x-1, y) && Tiles[x - 1, y].GetComponent<Tile>().isWalkable())
        {
            neighbors.Add(Tiles[x-1, y]);
        }
        if (IsWithinBounds(x + 1, y) && Tiles[x + 1, y].GetComponent<Tile>().isWalkable())
        {
            neighbors.Add(Tiles[x + 1, y]);
        }
        if (IsWithinBounds(x, y - 1) && Tiles[x, y - 1].GetComponent<Tile>().isWalkable())
        {
            neighbors.Add(Tiles[x, y - 1]);
        }
        if (IsWithinBounds(x, y + 1) && Tiles[x, y + 1].GetComponent<Tile>().isWalkable())
        {
            neighbors.Add(Tiles[x, y + 1]);
        }
        return neighbors;
    }

    public List<Tile> GetNeighborsIncBlocked(MapPosition pos)
    {
        return GetNeighborsIncBlocked(pos.X, pos.Y);
    }

    public List<Tile> GetNeighborsIncBlocked(int x, int y)
    {
        List<Tile> neighbors = new List<Tile>();
        if (IsWithinBounds(x - 1, y))
        {
            neighbors.Add(Tiles[x - 1, y]);
        }
        if (IsWithinBounds(x + 1, y))
        {
            neighbors.Add(Tiles[x + 1, y]);
        }
        if (IsWithinBounds(x, y - 1))
        {
            neighbors.Add(Tiles[x, y - 1]);
        }
        if (IsWithinBounds(x, y + 1) )
        {
            neighbors.Add(Tiles[x, y + 1]);
        }
        return neighbors;
    }

    public bool AreNeighbors(MapPosition p0, MapPosition p1)
    {
        List<Tile> neighbors = GetNeighborsIncBlocked(p0);
        Tile second = TileAtMapPosition(p1);
        return neighbors.Contains(second);
    }

    public Vector2 MapPosition(float x, float y)
    {
        int newX = (int)(x - transform.position.x);
        int newY = (int)(y - transform.position.y);
        Vector2 pos = new Vector2(newX, newY);
        return pos;
    }

    public Vector2 MapPosition(Vector2 pos)
    {
        return MapPosition(pos.x, pos.y);
    }

    public Tile TileAtMapPosition(MapPosition pos)
    {
        return tiles[pos.X, pos.Y];
    }

    private void Start()
    {
        EventManager.Instance.Raise(new MapChangeEvent(this));
        //GenerateTiles();
    }

    private void Update()
    {
		
	}

    public void BuildMap(MapTiles mapTiles)
    {
        
        int height = mapTiles.Tiles.Length;
        int width = mapTiles.Tiles[0].Row.Length;
        tiles = new Tile[width, height];
        for (int i = 0; i < height; i++)
        {
            TileType[] row = mapTiles.Tiles[i].Row;
            for (int j = 0; j < width; j++)
            {
                BuildTile(row[j], j, i);
            }
        }
        CreateBattleGroups(2, 2);
    }

    private void BuildTile(TileType tile, int x, int y)
    {
        Tile placedTile = null;
        switch (tile)
        {
            case TileType.Ground:
                placedTile = PlaceTile(x, y, templateTile);
                break;
            case TileType.Wall:
                placedTile = PlaceTile(x, y, templateBlockedTile);
                break;
            case TileType.GoodSpawn:
                placedTile = PlaceTile(x, y, templateTile);
                goodSpawn.Add(new MapPosition(x, y));
                break;
            case TileType.BadSpawn:
                placedTile  = PlaceTile(x, y, templateTile);
                badSpawn.Add(new MapPosition(x, y));
                break;
            default:
                Debug.Log("Not a correct TileType value");
                break;
        }
        if (placedTile != null)
        {
            placedTile.MapPosition = new MapPosition(x, y);
            tiles[x, y] = placedTile;
        }
    }

    private void CreateBattleGroups(int good, int bad)
    {
        BattleGroup g = CreateBattleGroup(true, good);
        BattleGroup b = CreateBattleGroup(false, bad);
        EventManager.Instance.Raise(new SetBattleGroupsEvent(g, b));
    }

    private BattleGroup CreateBattleGroup(bool isGood, int amount)
    {
        List<Character> members = new List<Character>();
        for (int i = 0; i < amount; i++)
        {
            Character c = SpawnCharacter(isGood);
            if (!isGood)
            {
                c.GetComponent<SpriteRenderer>().color = Color.yellow;
                c.gameObject.AddComponent<EnemyAI>();
            }
            members.Add(c);
        }
        return new BattleGroup(members, isGood);
    }

    private Character SpawnCharacter(bool isGood)
    {
        Character character = PlaceObject(0, 0, -.2f, baseCharacter)
            .GetComponent<Character>();
        string alignmentName = isGood ? "Good" : "Bad";
        character.gameObject.name =
            alignmentName + " Character " + Random.Range(100, 999);
        //p.transform.rotation = Quaternion.Euler(-15, 0, 0);
        List<MapPosition> spawnSpots = isGood ? goodSpawn : badSpawn;
        bool placed = false;
        foreach (MapPosition spot in spawnSpots)
        {
            if (TileAtMapPosition(spot).MoveObjectTo(character.gameObject))
            {
                placed = true;
                break;
            }
        }
        if (!placed)
        {
            Debug.Log("Couldn't place character");
            return null;
        }
        else
        {
            return character;
        }
    }

    private Tile PlaceTile(float x, float y, Tile tile)
    {
        float adjustedX = transform.position.x + x;
        float adjustedY = transform.position.y + y;
        return PlaceObject(adjustedX, adjustedY, 0, tile.gameObject).GetComponent<Tile>();
    }

    private GameObject PlaceObject(float x, float y, float z, GameObject obj)
    {
        return Instantiate(obj, new Vector3(x, y, z), Quaternion.identity, transform);
    }

    private Tile RandomTile()
    {
        int rngX = (int)(Random.value * width);
        int rngY = (int)(Random.value * height);
        return tiles[rngX, rngY].GetComponent<Tile>();
    }

}
