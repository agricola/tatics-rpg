using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    [SerializeField]
    private GameObject map;
    [SerializeField]
    private GameObject pathfindingManager;

    private Path path;
    private List<Tile> oldPath = new List<Tile>();
    private int limit;

    private void Awake()
    {
        EventManager.Instance.AddListener<PathfindEvent>(OnPathfindEvent);
        EventManager.Instance.AddListener<MoveCharacterEvent>(OnMoveEvent);
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener<PathfindEvent>(OnPathfindEvent);
        EventManager.Instance.RemoveListener<MoveCharacterEvent>(OnMoveEvent);
    }

    private void Start()
	{
        pathfindingManager = GameObject.Find("PathfindingManager");
    }
	
	private void Update()
	{
		if (!map) map = GameManager.Map.gameObject;
    }

    private void OnPathfindEvent(PathfindEvent e)
    {
        if (e is PathfindCreateEvent)
        {
            PathfindCreateEvent ev = e as PathfindCreateEvent;
            limit = ev.limit;
            FindPath(ev.source, ev.goal);
        }
        else if (e is CancelPathfindEvent)
        {
            ResetPath(true);
        }
    }

    private void FindPath(GameObject source, GameObject goal)
    {
        Pathfinder p = pathfindingManager.GetComponent<Pathfinder>();
        Vector2 sPos = source.transform.localPosition;
        Vector2 gPos = goal.transform.localPosition;

        ResetPath();
        path = p.GetPath(map.GetComponent<Map>(), sPos.x, sPos.y, gPos.x, gPos.y, limit);
        HighlightTiles(false);
    }

    private void ResetPath(bool noHighlight = false)
    {
        HighlightType h = noHighlight ? HighlightType.None : HighlightType.Old;
        if (oldPath.Count <= 0) return;
        foreach (var tile in oldPath)
        {
            tile.Highlight(h);
        }
        oldPath = new List<Tile>();
        path = null;
    }

    private void HighlightTiles(bool highlight)
    {
        if (path != null)
        {
            path.Current.Value.GetComponent<Tile>().Highlight(HighlightType.Targeting);
            oldPath.Add(path.Current.Value.GetComponent<Tile>());
            while (path.Advance())
            {
                path.Current.Value.GetComponent<Tile>().Highlight(HighlightType.Targeting);
                oldPath.Add(path.Current.Value.GetComponent<Tile>());
            }
        }
    }

    private void OnMoveEvent(MoveCharacterEvent e)
    {
        LinkedList<Tile> movement = path.Tiles;
        ResetPath(true);
        EventManager.Instance.Raise(new InputToggleEvent(false));
        StartCoroutine(MoveCharacter(movement, e.character));
    }

    private IEnumerator MoveCharacter(LinkedList<Tile> tiles, Character c)
    {
        foreach (var tile in tiles)
        {
            yield return new WaitForSeconds(0.2f);
            if (!tile.MoveObjectTo(c.gameObject))
            {
                break;
            }
        }
        EventManager.Instance.Raise(new InputToggleEvent(true));
    }
}
