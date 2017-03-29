using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private int turnNumber;
    private BattleGroup good;
    private BattleGroup bad;
    private BattleGroup current;
    private List<Character> actingBaddies = new List<Character>();

	private void Start()
	{
        turnNumber = 0;
        EventManager.Instance.AddListener<EndTurnEvent>(OnEndTurn);
        EventManager.Instance.AddListener<SetBattleGroupsEvent>(OnSetBattleGroups);
        EventManager.Instance.AddListener<AnimationEvent>(OnAnimationEvent);
    }

    private void OnDestroy()
    {
        EventManager em = EventManager.Instance;
        if (em)
        {
            em.RemoveListener<EndTurnEvent>(OnEndTurn);
            em.RemoveListener<SetBattleGroupsEvent>(OnSetBattleGroups);
            em.RemoveListener<AnimationEvent>(OnAnimationEvent);
        }
    }

    public void Initialize(BattleGroup good, BattleGroup bad)
    {
        this.good = good;
        this.bad = bad;
        current = good;
    }

    private void OnSetBattleGroups(SetBattleGroupsEvent e)
    {
        Initialize(e.good, e.bad);
    }

    private void OnEndTurn(EndTurnEvent e)
    {
        ChangeTurn();
    }

    private void ChangeTurn()
    {
        EventManager.Instance.Raise(new CombatMenuEvent());
        if (current == bad)
        {
            EventManager.Instance.Raise(new InputToggleEvent(true));
            good.ResetAllActions();
            turnNumber++;
            current = good;
            //Debug.Log("good turn");
        }
        else
        {
            EventManager.Instance.Raise(new InputToggleEvent(false));
            bad.ResetAllActions();
            current = bad;
            ExecuteEnemyTurns();
            //Debug.Log("bad turn");
            //StartCoroutine(EvilScheming());
        }
    }

    private void OnAnimationEvent(AnimationEvent e)
    {
        if (e is ToggleWalkEvent)
        {
            OnWalkToggle(e as ToggleWalkEvent);
        }
    }

    private void ExecuteEnemyTurns()
    {
        Map map = GameManager.Instance.Map;
        Debug.Log("good memers : " + good.Members.Count);
        //AIManager.Instance.DetermineStrategies(good.Members, bad.Members, map);
        actingBaddies = bad.Members;
        ActivateNextBaddieTurn();
    }

    private void OnWalkToggle(ToggleWalkEvent e)
    {
        if (actingBaddies.Count <= 0) return;
        Debug.Log("toggle! " + !e.Act + " " + (e.Actor == actingBaddies[0].gameObject));
        if (!e.Act && e.Actor == actingBaddies[0].gameObject)
        {
            actingBaddies.RemoveAt(0);
            if (!BaddiesFinished())
            {
                Debug.Log("next baddie");
                ActivateNextBaddieTurn();
            }
            
        }
    }

    private bool BaddiesFinished()
    {
        bool finished = false;
        if (actingBaddies.Count <= 0)
        {
            ChangeTurn();
            finished = true;
        }
        return finished;
    }

    private void ActivateNextBaddieTurn()
    {
        Map map = GameManager.Instance.Map;
        Character actor = actingBaddies[0];
        EnemyAI ai = actor.GetComponent<EnemyAI>();
        ai.DetermineStrategy(good.Members, map);
        Path path = ai.WalkPath;
        bool skip = path == null;
        if (skip)
        {
            actingBaddies.RemoveAt(0);
            if (!BaddiesFinished()) ActivateNextBaddieTurn();
            return;
        }
        EventManager.Instance.Raise<AnimationEvent>(new ToggleWalkEvent(true, actor.gameObject));
        StartCoroutine(CommandManager.Instance.MoveCharacter(path.Tiles, actor));
    }
}
