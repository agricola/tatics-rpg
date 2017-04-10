using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
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

    private void OnDisable()
    {
        EventManager em = EventManager.Instance;
        if (em)
        {
            em.RemoveListener<PathfindEvent>(OnPathfindEvent);
            em.RemoveListener<MoveCharacterEvent>(OnMoveEvent);
            em.RemoveListener<FightEvent>(OnFight);
        }
    }

    private void Start()
	{
        pathfinder = Pathfinder.Instance;
        EventManager.Instance.AddListener<PathfindEvent>(OnPathfindEvent);
        EventManager.Instance.AddListener<MoveCharacterEvent>(OnMoveEvent);
        EventManager.Instance.AddListener<FightEvent>(OnFight);
    }

    private void OnPathfindEvent(PathfindEvent e)
    {
        if (e is PathfindCreateEvent)
        {
            if (findingPath) return;
            findingPath = true;
            PathfindCreateEvent ev = e as PathfindCreateEvent;
            int limit = ev.limit;
            ResetPath();
            Map map = GameManager.Instance.CurrentMap;
            path = FindPath(ev.source, ev.goal, limit, map);
            HighlightPathTiles();
            findingPath = false;
        }
        else if (e is CancelPathfindEvent)
        {
            ResetPath();
        }
    }

    private void OnFight(FightEvent e)
    {
        AttackCommand(e.Attacker, e.Defender);
    }

    public void AttackCommand(Character attacker, Character defender)
    {
        EventManager.Instance.Raise<AnimationEvent>(new AnimationFightEvent(
            AnimationStatus.Start,
            attacker,
            defender));
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

    private void ResetPath()
    {
        oldPath = new List<Tile>();
        path = null;
        //HighlightEvent e = new HighlightEvent(HighlightSelection.Sub);
        //EventManager.Instance.Raise<HighlightEvent>(e);
    }

    private void HighlightPathTiles()
    {
        if (path == null || path.Current == null) return;
        List<Tile> tiles = (path.Tiles).ToList();
        UpdateTilesEvent e = new UpdateTilesEvent(HighlightSelection.Sub, tiles, true);
        EventManager.Instance.Raise<HighlightEvent>(e);
    }

    private void OnMoveEvent(MoveCharacterEvent e)
    {
        if (e.Character.IsGood)
        {

        }
        if (!e.Skip)
        {
            if (path == null)
            {
                return;
            }
            IEnumerable<Tile> movement = path.Tiles;
            if (e.Character.IsGood) ResetPath();
            e.Character.Moved = true;
            EventManager.Instance.Raise<AnimationEvent>(new AnimationWalkEvent(AnimationStatus.Start, e.Character.gameObject));
            StartCoroutine(MoveCharacter(movement, e.Character));
        }
        else
        {
            if (e.Character.IsGood)
            {
                e.Character.Moved = true;
                EventManager.Instance.Raise(new CharacterStateTransitionEvent(new ActionState()));
            }
        }
    }

    public IEnumerator MoveCharacter(IEnumerable<Tile> tiles, Character c)
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
        EventManager.Instance.Raise<AnimationEvent>(new AnimationWalkEvent(AnimationStatus.Finish, c.gameObject));
    }
}
