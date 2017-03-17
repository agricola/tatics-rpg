using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField]
    GameObject baseTile;
    [SerializeField]
    GameObject baseBlockedTile;
    [SerializeField]
    private int width = 0;
    [SerializeField]
    private int height = 0;
    [SerializeField]
    private GameObject[,] tiles = new GameObject[0,0];

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
    public GameObject[,] Tiles
    {
        get
        {
            return tiles;
        }
    } 

	private void Start()
    {
        GenerateTiles();
    }
	
	private void Update()
    {
		
	}

    private void GenerateTiles()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        tiles = new GameObject[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                float x = i + transform.position.x;
                float y = j + transform.position.y;
                GameObject template = Random.value > .7 ? baseBlockedTile : baseTile;
                GameObject tile = Instantiate(template, new Vector2(x, y), Quaternion.identity, transform);
                tiles[i, j] = tile;
            }
        }
    }
}
