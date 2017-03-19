﻿using System.Collections;
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
    private Character baseCharacter;

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

    public List<GameObject> GetNeighbors(int x, int y)
    {
        List<GameObject> neighbors = new List<GameObject>();
        /*for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y - 1; j <= y + 1; j++)
            {
                if (IsWithinBounds(i, j) && !Tiles[i, j].GetComponent<Tile>().Blocked)
                {
                    neighbors.Add(Tiles[i, j]);
                }
            }
        }*/
        if (IsWithinBounds(x-1, y) && !Tiles[x - 1, y].GetComponent<Tile>().Blocked)
        {
            neighbors.Add(Tiles[x-1, y].gameObject);
        }
        if (IsWithinBounds(x + 1, y) && !Tiles[x + 1, y].GetComponent<Tile>().Blocked)
        {
            neighbors.Add(Tiles[x + 1, y].gameObject);
        }
        if (IsWithinBounds(x, y - 1) && !Tiles[x, y - 1].GetComponent<Tile>().Blocked)
        {
            neighbors.Add(Tiles[x, y - 1].gameObject);
        }
        if (IsWithinBounds(x, y + 1) && !Tiles[x, y + 1].GetComponent<Tile>().Blocked)
        {
            neighbors.Add(Tiles[x, y + 1].gameObject);
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
                Tile template = Random.value > .7 ? templateBlockedTile : templateTile;
                //template.MapPosition = new MapPosition(i, j);
                GameObject tile = PlaceTile(x, y, template);
                tile.GetComponent<Tile>().MapPosition = new MapPosition(i, j);
                tiles[i, j] = tile.GetComponent<Tile>();
            }
        }
        SpawnPlayer();
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        Tile placedTile = RandomTile();
        GameObject p = PlaceObject(0, 0, -0.01f, baseCharacter.gameObject);
        while (!placedTile.MoveObjectTo(p))
        {
            placedTile = RandomTile();
        }
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