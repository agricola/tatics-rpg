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
        GenerateTiles();
        
    }

    private void Update()
    {
		
	}

    private void GenerateTiles()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        tiles = new Tile[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                float x = i + transform.position.x;
                float y = j + transform.position.y;
                Tile template = Random.value > .8 ? templateBlockedTile : templateTile;
                //template.MapPosition = new MapPosition(i, j);
                GameObject tile = PlaceTile(x, y, template);
                tile.GetComponent<Tile>().MapPosition = new MapPosition(i, j);
                tiles[i, j] = tile.GetComponent<Tile>();
            }
        }
        CreateBattleGroups(2, 2);
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
            Character c = SpawnCharacter();
            c.IsGood = isGood;
            if (!isGood)
            {
                c.GetComponent<SpriteRenderer>().color = Color.yellow;
                c.gameObject.AddComponent<EnemyAI>();
            }
            members.Add(c);
        }
        return new BattleGroup(members, isGood);
    }

    private Character SpawnCharacter()
    {
        Tile placedTile = RandomTile();
        GameObject p = PlaceObject(0, 0, -.2f, baseCharacter);
        p.name = "Character " + Random.Range(100, 999); 
        //p.transform.rotation = Quaternion.Euler(-15, 0, 0);
        while (!placedTile.MoveObjectTo(p))
        {
            placedTile = RandomTile();
        }
        return p.GetComponent<Character>();
    }

    private GameObject PlaceTile(float x, float y, Tile tile)
    {
        return PlaceObject(x, y, 0, tile.gameObject);
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
