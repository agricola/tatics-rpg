using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    private GameObject map;
    private Pathfinder pathfinder;

    private Path path;
    private List<Tile> oldPath = new List<Tile>();
    private bool findingPath = false;

    static CommandManager instance;
    public static CommandManager Instance
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

    private void OnDestroy()
    {
        //ResetPath();
        EventManager.Instance.RemoveListener<PathfindEvent>(OnPathfindEvent);
        EventManager.Instance.RemoveListener<MoveCharacterEvent>(OnMoveEvent);
        EventManager.Instance.RemoveListener<FightEvent>(OnFight);
    }

    private void Start()
	{
        pathfinder = Pathfinder.Instance;
        EventManager.Instance.AddListener<PathfindEvent>(OnPathfindEvent);
        EventManager.Instance.AddListener<MoveCharacterEvent>(OnMoveEvent);
        EventManager.Instance.AddListener<FightEvent>(OnFight);
    }
	
	private void Update()
	{
		if (!map) map = GameManager.Instance.Map.gameObject;
    }

    private void OnPathfindEvent(PathfindEvent e)
    {
        if (e is PathfindCreateEvent)
        {
            if (findingPath) return;
            findingPath = true;
            PathfindCreateEvent ev = e as PathfindCreateEvent;
            int limit = ev.limit;
            ResetOldPath();
            Map map = GameManager.Instance.Map;
            path = FindPath(ev.source, ev.goal, limit, map);
            HighlightTiles();
            findingPath = false;
        }
        else if (e is CancelPathfindEvent)
        {
            CancelPathfind();
        }
    }

    private void OnFight(FightEvent e)
    {
        AttackCommand(e.Attacker, e.Defender);
    }

    public void AttackCommand(Character attacker, Character defender)
    {
        MapPosition ap = attacker.MapPosition;
        MapPosition dp = defender.MapPosition;
        float xDiff = dp.X - ap.X;
        float yDiff = dp.Y - ap.Y;
        float x = xDiff == 0 ? 0 : xDiff / 5;
        float y = yDiff == 0 ? 0 : yDiff / 5;
        Vector2 direction = new Vector2(x, y);
        int dmg = attacker.Damage;
        attacker.GetComponent<AnimationManager>().TriggerFightAnimation(
            direction,
            defender.gameObject,
            dmg);
    }

    private void CancelPathfind()
    {
        ResetPath();
    }

    public Path FindPath(Tile source, Tile goal, int limit, Map map)
    {
        MapPosition sPos = source.MapPosition;
        MapPosition gPos = goal.MapPosition;
        Path path;
        path = pathfinder.GetPath(map, sPos.X, sPos.Y, gPos.X, gPos.Y, limit);
        if (path == null) return null;
        if (!path.Tiles.Contains(goal)) path = null;
        return path;
    }
    
    private void ResetOldPath()
    {
        if (oldPath.Count <= 0) return;
        foreach (var tile in oldPath)
        {
            if (tile) tile.Highlight(HighlightType.Old);
        }
        oldPath = new List<Tile>();
        path = null;
    }

    private void ResetPath()
    {
        oldPath = new List<Tile>();
        path = null;
        EventManager.Instance.Raise(new UnhighlightTilesEvent());
    }

    private void HighlightTiles()
    {
        if (path != null)
        {
            if (path.Current == null) return;
            path.Current.Value.Highlight(HighlightType.Targeting);
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
        if (!e.Skip)
        {
            if (path == null)
            {
                return;
            }
            LinkedList<Tile> movement = path.Tiles;
            if (e.Character.IsGood) ResetPath();
            EventManager.Instance.Raise<AnimationEvent>(new ToggleWalkEvent(true, e.Character.gameObject));
            StartCoroutine(MoveCharacter(movement, e.Character));
        }
        else
        {
            if (e.Character.IsGood) EventManager.Instance.Raise(new CharacterStateTransitionEvent(new ActionState()));
        }
        e.Character.Moved = true;
    }

    public IEnumerator MoveCharacter(LinkedList<Tile> tiles, Character c)
    {
        foreach (var tile in tiles)
        {
            yield return new WaitForSeconds(0.2f);
            if (!tile.MoveObjectTo(c.gameObject))
            {
                break;
            }
        }
        if (c.IsGood)
        {
            EventManager.Instance.Raise(new InputToggleEvent(true));
            EventManager.Instance.Raise(new CharacterStateTransitionEvent(new ActionState()));
        }
        EventManager.Instance.Raise<AnimationEvent>(new ToggleWalkEvent(false, c.gameObject));
    }
}
