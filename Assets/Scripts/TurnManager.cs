using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurnManager : MonoBehaviour
{
    private int turnNumber;
    private BattleGroup good;
    private BattleGroup bad;
    private BattleGroup current;
    private AIManager aiManager;

	private void Start()
	{
        turnNumber = 0;
        EventManager em = EventManager.Instance;
        if (em)
        {
            em.AddListener<EndTurnEvent>(OnEndTurn);
            em.AddListener<SetBattleGroupsEvent>(OnSetBattleGroups);
            em.AddListener<CharacterChangeEvent>(OnCharacterChange);
            em.AddListener<EnemyTurnEvent>(OnEnemyTurnEvent);
        }
        else
        {
            Debug.Log("event manager not found");
        }
        aiManager = AIManager.Instance;
    }

    private void OnDisable()
    {
        EventManager em = EventManager.Instance;
        if (em)
        {
            em.RemoveListener<EndTurnEvent>(OnEndTurn);
            em.RemoveListener<SetBattleGroupsEvent>(OnSetBattleGroups);
            em.RemoveListener<CharacterChangeEvent>(OnCharacterChange);
            em.RemoveListener<EnemyTurnEvent>(OnEnemyTurnEvent);
        }
    }

    public void Initialize(BattleGroup good, BattleGroup bad)
    {
        this.good = good;
        this.bad = bad;
        current = good;
    }

    private void OnEnemyTurnEvent(EnemyTurnEvent e)
    {
        if (e.Status == EventStatus.Finish)
        {
            ChangeTurn();
        }
    }

    private void OnCharacterChange(CharacterChangeEvent e)
    {
        Debug.Log("char change");
        //if (!e.create) CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        Debug.Log("good: " + good.Members.Count + ", bad: " + bad.Members.Count);
        if (good.Members.Count <= 0)
        {
            Debug.Log("you lose!");
            SceneManager.LoadScene("End");
        }
        else if (bad.Members.Count <= 0)
        {
            Debug.Log("you win!");
            SceneManager.LoadScene("End");
        }
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
        CheckWinCondition();
        if (current == bad)
        {
            EventManager.Instance.Raise(new InputToggleEvent(true));
            good.ResetAllActions();
            turnNumber++;
            current = good;
        }
        else
        {
            EventManager.Instance.Raise(new InputToggleEvent(false));
            bad.ResetAllActions();
            current = bad;
            EventManager.Instance.Raise(new EnemyTurnEvent(good, bad, EventStatus.Start));
            //AIManager.Instance.ExecuteEnemyTurns(good, bad, UpdateGoodGroup);
        }
    }

}
