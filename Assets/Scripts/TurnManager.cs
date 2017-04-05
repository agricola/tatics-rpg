using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurnManager : MonoBehaviour
{
    private int turnNumber;
    [SerializeField]
    private BattleGroup good;
    [SerializeField]
    private BattleGroup bad;
    [SerializeField]
    private BattleGroup current;

	private void Start()
	{
        turnNumber = 0;
        EventManager em = EventManager.Instance;
        if (em)
        {
            em.AddListener<EndTurnEvent>(OnEndTurn);
            em.AddListener<SetBattleGroupsEvent>(OnSetBattleGroups);
            em.AddListener<EnemyTurnEvent>(OnEnemyTurnEvent);
            em.AddListener<CharacterDeathEvent>(OnCharacterDeath);
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
            em.RemoveListener<EndTurnEvent>(OnEndTurn);
            em.RemoveListener<SetBattleGroupsEvent>(OnSetBattleGroups);
            em.RemoveListener<EnemyTurnEvent>(OnEnemyTurnEvent);
            em.RemoveListener<CharacterDeathEvent>(OnCharacterDeath);
        }
    }

    private void OnCharacterDeath(CharacterDeathEvent e)
    {
        CheckWinCondition(e.DeadCharacter.GetComponent<Character>());
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

    private void CheckWinCondition(Character justDied)
    {
        if (good.Members.Count == 1 && good.Members[0] == justDied)
        {
            Debug.Log("you lose!");
            SceneManager.LoadScene("End");
        }
        else if (bad.Members.Count == 1 && bad.Members[0] == justDied)
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
        ChangeTurn(true);
    }

    private void ChangeTurn(bool endGoodTurn = false)
    {
        if (endGoodTurn) current = good;
        if (current == bad)
        {
            EventManager.Instance.Raise(new InputToggleEvent(true));
            good.ResetAllActions();
            turnNumber++;
            current = good;
        }
        else
        {
            bad.ResetAllActions();
            current = bad;
            EventManager.Instance.Raise(new EnemyTurnEvent(good, bad, EventStatus.Start));
        }
    }

}
