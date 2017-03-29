using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour {
    private List<Character> actingBaddies = new List<Character>();
    private BattleGroup good;
    static AIManager instance;
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

    private void OnDestroy()
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

    public void ExecuteEnemyTurns(BattleGroup good, BattleGroup bad)
    {
        this.good = good;
        Map map = GameManager.Instance.Map;
        Debug.Log("good memers : " + good.Members.Count);
        //AIManager.Instance.DetermineStrategies(good.Members, bad.Members, map);
        actingBaddies = bad.Members;
        ActivateNextBaddieTurn();
    }

    private void OnAnimationEvent(AnimationEvent e)
    {
        if (e is ToggleWalkEvent)
        {
            OnWalkToggle(e as ToggleWalkEvent);
        }
    }

    private void OnWalkToggle(ToggleWalkEvent e)
    {
        if (actingBaddies.Count <= 0) return;
        if (!e.Act && e.Actor == actingBaddies[0].gameObject)
        {
            ExecuteActionStrategy(actingBaddies[0].GetComponent<EnemyAI>(), GameManager.Instance.Map);
            //actingBaddies.RemoveAt(0);
            /*if (!BaddiesFinished())
            {*/
            //}

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
        ExecuteMoveStrategy(actor.GetComponent<EnemyAI>(), map);
    }

    private void ExecuteMoveStrategy(EnemyAI ai, Map map)
    {
        Path path = ai.DetermineMoveStrategy(good.Members, map);
        bool skip = path == null;
        if (skip)
        {
            /*actingBaddies.RemoveAt(0);
            if (!BaddiesFinished()) ActivateNextBaddieTurn();
            return;*/
            ExecuteActionStrategy(ai, map);
            return;
        }
        EventManager.Instance.Raise<AnimationEvent>(new ToggleWalkEvent(true, ai.gameObject));
        StartCoroutine(CommandManager.Instance.MoveCharacter(path.Tiles, ai.GetComponent<Character>()));
    }

    private void ExecuteActionStrategy(EnemyAI ai, Map map)
    {
        Character target = ai.DetermineActionStrategy(good.Members, map);
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
