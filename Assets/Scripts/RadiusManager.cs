using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RadiusManager : MonoBehaviour
{
    private List<Tile> currentRadius;
    private Tile currentCenter;
    

    private void Start()
    {
        EventManager.Instance.AddListener<RadiusEvent>(OnRadiusEvent);
        EventManager.Instance.AddListener<MoveCharacterEvent>(OnCharacterMove);
    }

    private void OnDestroy()
    {
        EventManager em = EventManager.Instance;
        if (em)
        {
            em.RemoveListener<RadiusEvent>(OnRadiusEvent);
            em.RemoveListener<MoveCharacterEvent>(OnCharacterMove);
        }
    }

    private void OnCharacterMove(MoveCharacterEvent e)
    {
        EventManager.Instance.Raise(new UnhighlightTilesEvent());
    }

    private void OnRadiusEvent(RadiusEvent e)
    {
        //Debug.Log("radius event start");
        if (e is CreateRadiusEvent)
        {
            OnCreateRadius(e as CreateRadiusEvent);
        } else if (e is DestroyRadiusEvent)
        {
            OnDestroyRadius(e as DestroyRadiusEvent);
        }
        //Debug.Log("radius event end");
    }

    private void OnDestroyRadius(DestroyRadiusEvent e)
    {
        currentCenter = null;
        //if (currentRadius != null) ToggleHighlightRadius(false);
        currentRadius = null;
    }

    private void OnCreateRadius(CreateRadiusEvent e)
    {
        Map map = GameManager.Map;
        MapPosition centerPosition =
            MapPosition.VectorToMapPosition(e.character.gameObject.transform.localPosition);
        Tile centerTile = map.TileAtMapPosition(centerPosition);
        currentCenter = centerTile;
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
