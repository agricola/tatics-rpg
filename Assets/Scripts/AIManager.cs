using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour {
    private List<Character> actingBaddies = new List<Character>();
    static AIManager instance;
    Func<BattleGroup> updateGroup;

    public static AIManager Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
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

    private void Start()
    {
        EventManager.Instance.AddListener<AnimationEvent>(OnAnimationEvent);
        EventManager.Instance.AddListener<FinishCombatEvent>(OnCombatFinish);
    }

    private void OnDisable()
    {
        EventManager em = EventManager.Instance;
        if (em)
        {
            em.RemoveListener<AnimationEvent>(OnAnimationEvent);
            em.RemoveListener<FinishCombatEvent>(OnCombatFinish);
        }
    }
    /*
    public void DetermineStrategies(List<Character> good, List<Character> bad, Map map)
    {
        foreach (var enemy in bad)
        {
            enemy.GetComponent<EnemyAI>().DetermineStrategy(good,map);
        }
    }*/

    public void ExecuteEnemyTurns(BattleGroup good, BattleGroup bad, Func<BattleGroup> updateGroup)
    {
        Map map = GameManager.Instance.Map;
        //AIManager.Instance.DetermineStrategies(good.Members, bad.Members, map);
        actingBaddies = bad.Members;
        this.updateGroup = updateGroup;
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
            ExecuteActionStrategy(actingBaddies[0].GetComponent<EnemyAI>(), GameManager.Instance.Map);
        }
    }

    private bool BaddiesFinished()
    {
        bool finished = false;
        if (actingBaddies.Count <= 0)
        {
            EventManager.Instance.Raise(new ChangeTurnEvent());
            finished = true;
        }
        return finished;
    }

    private void ActivateNextBaddieTurn()
    {
        Map map = GameManager.Instance.Map;
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
        List<Character> good = updateGroup().Members;
        Path path = ai.DetermineMoveStrategy(good, map);
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
        List<Character> good = updateGroup().Members;
        Character target = ai.DetermineActionStrategy(good, map);
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
            if (!BaddiesFinished()) ActivateNextBaddieTurn();
        }
    }
}
