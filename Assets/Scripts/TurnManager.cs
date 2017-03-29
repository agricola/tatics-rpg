using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private int turnNumber;
    private BattleGroup good;
    private BattleGroup bad;
    private BattleGroup current;

	private void Start()
	{
        turnNumber = 0;
        EventManager.Instance.AddListener<EndTurnEvent>(OnEndTurn);
        EventManager.Instance.AddListener<SetBattleGroupsEvent>(OnSetBattleGroups);
        EventManager.Instance.AddListener<ChangeTurnEvent>(OnChangeTurnEvent);
    }

    private void OnDestroy()
    {
        EventManager em = EventManager.Instance;
        if (em)
        {
            em.RemoveListener<EndTurnEvent>(OnEndTurn);
            em.RemoveListener<SetBattleGroupsEvent>(OnSetBattleGroups);
            em.RemoveListener<ChangeTurnEvent>(OnChangeTurnEvent);
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
            AIManager.Instance.ExecuteEnemyTurns(good, bad);
            //Debug.Log("bad turn");
            //StartCoroutine(EvilScheming());
        }
    }

    private void OnChangeTurnEvent(ChangeTurnEvent e)
    {
        ChangeTurn();
    }


}
