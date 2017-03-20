using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RadiusManager : MonoBehaviour
{
    private List<Tile> currentRadius;
    private Tile currentCenter;

    private void Awake()
    {
        EventManager.Instance.AddListener<CharacterSelectEvent>(OnCharacterSelect);
        EventManager.Instance.AddListener<MoveCharacterEvent>(OnCharacterMove);
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener<CharacterSelectEvent>(OnCharacterSelect);
        EventManager.Instance.RemoveListener<MoveCharacterEvent>(OnCharacterMove);
    }

    private void OnCharacterMove(MoveCharacterEvent e)
    {
        EventManager.Instance.Raise(new UnhighlightTilesEvent());
    }

    private void OnCharacterSelect(CharacterSelectEvent e)
    {/*
        if (!e.select)
        {
            ToggleHighlightRadius(false);
        }*/
        EventManager.Instance.Raise<PathfindEvent>(new CancelPathfindEvent());
        EventManager.Instance.Raise(new UnhighlightTilesEvent());
        
        Map map = GameManager.Map;
        MapPosition centerPosition =
            MapPosition.VectorToMapPosition(e.character.gameObject.transform.localPosition);
        Tile centerTile = map.TileAtMapPosition(centerPosition);
        if (currentCenter == centerTile)
        {
            currentCenter = null;
            return;
        }
        currentCenter = centerTile;
        currentRadius = null;
        currentRadius = GenerateRadius(centerTile, e.character.MovementLimit, map);
        ToggleHighlightRadius(true);
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
