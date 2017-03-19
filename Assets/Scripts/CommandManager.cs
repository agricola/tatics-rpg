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

    // Commands



    // Unity stuff
    private void Awake()
    {
        EventManager.Instance.AddListener<PathfindEvent>(OnPathfindEvent);
        EventManager.Instance.AddListener<MoveCharacterEvent>(OnMoveEvent);
        EventManager.Instance.AddListener<DeselectEvent>(OnDeselectEvent);
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener<PathfindEvent>(OnPathfindEvent);
        EventManager.Instance.RemoveListener<DeselectEvent>(OnDeselectEvent);
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

    // Helpers

    private void OnDeselectEvent(DeselectEvent e)
    {
        ResetPath();
    } 

    private void OnPathfindEvent(PathfindEvent e)
    {
        if (e is PathfindCreateEvent)
        {
            PathfindCreateEvent ev = e as PathfindCreateEvent;
            FindPath(ev.source, ev.goal);
        }
    }

    private void FindPath(GameObject source, GameObject goal)
    {
        Pathfinder p = pathfindingManager.GetComponent<Pathfinder>();
        Vector2 sPos = source.transform.localPosition;
        Vector2 gPos = goal.transform.localPosition;

        ResetPath();
        path = p.GetPath(map.GetComponent<Map>(), sPos.x, sPos.y, gPos.x, gPos.y);
        HighlightTiles(true);
    }

    private void ResetPath()
    {
        foreach (var tile in oldPath)
        {
            tile.Highlight(false);
        }
        oldPath = new List<Tile>();
        path = null;
    }

    private void HighlightTiles(bool highlight)
    {
        if (path != null)
        {
            path.Current.Value.GetComponent<Tile>().Highlight(highlight);
            oldPath.Add(path.Current.Value.GetComponent<Tile>());
            while (path.Advance())
            {
                path.Current.Value.GetComponent<Tile>().Highlight(highlight);
                oldPath.Add(path.Current.Value.GetComponent<Tile>());
            }
        }
    }

    private void OnMoveEvent(MoveCharacterEvent e)
    {
        LinkedList<GameObject> movement = path.Tiles;
        ResetPath();
        EventManager.Instance.Raise(new InputToggleEvent(false));
        StartCoroutine(MoveCharacter(movement, e.character));
    }

    private IEnumerator MoveCharacter(LinkedList<GameObject> tiles, Character c)
    {
        foreach (var tile in tiles)
        {
            yield return new WaitForSeconds(0.2f);
            tile.GetComponent<Tile>().MoveObjectTo(c.gameObject);
        }
        EventManager.Instance.Raise(new InputToggleEvent(true));
    }
}
