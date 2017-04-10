using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIManager : MonoBehaviour {
   
    private List<Character> actingBaddies = new List<Character>();
    static AIManager instance;
    private BattleGroup goodGroup;
    private BattleGroup badGroup;

    public static AIManager Instance
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
    
    private void OnEnable()
    {
        EventManager em = EventManager.Instance;
        if (em)
        {
            em.AddListener<AnimationEvent>(OnAnimationEvent);
            em.AddListener<FinishCombatEvent>(OnCombatFinish);
            em.AddListener<EnemyTurnEvent>(OnEnemyTurnEvent);
        }
        else
        {
            Debug.Log("event manager not found");
        }
    }

    private void OnDisable()
    {
        EventManager em = EventManager.Instance;
        if (em)
        {
            em.RemoveListener<AnimationEvent>(OnAnimationEvent);
            em.RemoveListener<FinishCombatEvent>(OnCombatFinish);
            em.RemoveListener<EnemyTurnEvent>(OnEnemyTurnEvent);
        }
    }

    private void OnEnemyTurnEvent(EnemyTurnEvent e)
    {
        if (e.Status == EventStatus.Start)
        {
            ExecuteEnemyTurns(e.GoodGroup, e.BadGroup);
        }
    }

    private void ExecuteEnemyTurns(BattleGroup good, BattleGroup bad)
    {
        Map map = GameManager.Instance.CurrentMap;
        //AIManager.Instance.DetermineStrategies(good.Members, bad.Members, map);
        actingBaddies = bad.Members;
        goodGroup = good;
        badGroup = bad;
        ActivateNextBaddieTurn();
    }

    private void OnAnimationEvent(AnimationEvent e)
    {
        if (e is AnimationWalkEvent)
        {
            OnWalkToggle(e as AnimationWalkEvent);
        }
    }

    private void OnWalkToggle(AnimationWalkEvent e)
    {
        if (actingBaddies.Count <= 0) return;
        if (e.Status == AnimationStatus.Finish && e.Actor == actingBaddies[0].gameObject)
        {
            ExecuteActionStrategy(
                actingBaddies[0].GetComponent<EnemyAI>(),
                GameManager.Instance.CurrentMap);
        }
    }

    private bool BaddiesFinished()
    {
        bool finished = false;
        if (actingBaddies.Count <= 0)
        {
            EventManager.Instance.Raise(new EnemyTurnEvent(goodGroup, badGroup, EventStatus.Finish));
            finished = true;
        }
        return finished;
    }

    private void ActivateNextBaddieTurn()
    {
        Map map = GameManager.Instance.CurrentMap;
        Character actor = actingBaddies[0];
        Action execute = () => ExecuteMoveStrategy(actor.GetComponent<EnemyAI>(), map);
        WaitBeforeAction(execute, 0.5f);
    }

    // put this somewhere else to be accessible by other stuff probably
    private void WaitBeforeAction(Action action, float seconds)
    {
        StartCoroutine(WaitCoroutine(action, seconds));
    }

    private IEnumerator WaitCoroutine(Action action, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        action();
    }

    private void ExecuteMoveStrategy(EnemyAI ai, Map map)
    {
        Path path = ai.DetermineMoveStrategy(goodGroup.Members, map);
        bool skip = path == null || path.Tiles.Count == 1;
        if (skip)
        {
            ExecuteActionStrategy(ai, map);
            return;
        }
        EventManager.Instance.Raise<AnimationEvent>(new AnimationWalkEvent(AnimationStatus.Start, ai.gameObject));
        StartCoroutine(CommandManager.Instance.MoveCharacter(path.Tiles, ai.GetComponent<Character>()));
    }

    private void ExecuteActionStrategy(EnemyAI ai, Map map)
    {
        Character target = ai.DetermineActionStrategy(goodGroup.Members, map);
        bool skip = target == null;
        if (skip)
        {
            actingBaddies.RemoveAt(0);
            if (!BaddiesFinished()) ActivateNextBaddieTurn();
            return;
        }
        CommandManager.Instance.AttackCommand(ai.GetComponent<Character>(), target);
    }

    private void OnCombatFinish(FinishCombatEvent e)
    {
        if (actingBaddies.Count <= 0) return;
        if (e.Attacker == actingBaddies[0])
        {
            actingBaddies.RemoveAt(0);
            if (!BaddiesFinished())
            {
                ActivateNextBaddieTurn();
            }
        }
    }
}
