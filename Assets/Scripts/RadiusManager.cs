using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class RadiusManager : MonoBehaviour
{
    private List<Tile> currentRadius;

    static RadiusManager instance;
    public static RadiusManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public List<Tile> GetRadius(MapPosition center, int limit)
    {
        Map map = GameManager.Instance.CurrentMap;
        Tile centerTile = map.TileAtMapPosition(center);
        return GenerateRadius(centerTile, limit, map);
        //ToggleHighlightRadius(true);
    }

    private List<Tile> GenerateRadius(Tile center, int radius, Map map)
    {
        Queue<Tile> frontier = new Queue<Tile>();
        frontier.Enqueue(center);
        Dictionary<Tile, int> costSoFar = new Dictionary<Tile, int>();
        costSoFar[center] = 0;

        while (frontier.Count > 0)
        {
            Tile current = frontier.Dequeue();
            int cost = costSoFar[current] + 1;
            if (cost > radius) continue;
            foreach (var next in map.GetNeighbors(current.MapPosition))
            {
                if (!costSoFar.ContainsKey(next))
                {
                    frontier.Enqueue(next);
                    costSoFar[next] = cost;
                }
            }
        }
        return costSoFar.Keys.ToList();
    }

    private void ToggleHighlightRadius(bool highlight)
    {
        HighlightType h = highlight ? HighlightType.Radius : HighlightType.None;
        foreach (var tile in currentRadius)
        {
            tile.Highlight(h);
        }
    }
}
