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
    private bool canChangeCharacters = false;

	private void Start()
	{
        turnNumber = 0;
        EventManager.Instance.AddListener<EndTurnEvent>(OnEndTurn);
        EventManager.Instance.AddListener<SetBattleGroupsEvent>(OnSetBattleGroups);
        EventManager.Instance.AddListener<ChangeTurnEvent>(OnChangeTurnEvent);
        EventManager.Instance.AddListener<CharacterChangeEvent>(OnCharacterChange);
        aiManager = AIManager.Instance;
    }

    private void OnDisable()
    {
        canChangeCharacters = false;
        EventManager em = EventManager.Instance;
        if (em)
        {
            em.RemoveListener<EndTurnEvent>(OnEndTurn);
            em.RemoveListener<SetBattleGroupsEvent>(OnSetBattleGroups);
            em.RemoveListener<ChangeTurnEvent>(OnChangeTurnEvent);
            em.RemoveListener<CharacterChangeEvent>(OnCharacterChange);
        }
    }

    public void Initialize(BattleGroup good, BattleGroup bad)
    {
        this.good = good;
        this.bad = bad;
        current = good;
    }

    private void OnCharacterChange(CharacterChangeEvent e)
    {
        Debug.Log("change character");
        if (!canChangeCharacters) return;
        BattleGroup group = e.character.IsGood ? good : bad;
        Action<Character> change = e.create ? (Action<Character>)group.AddMember : group.RemoveMember;
        change(e.character);
        if (!e.create) CheckWinCondition();
    }

    private void CheckWinCondition()
    {
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
        canChangeCharacters = true;
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
        }
        else
        {
            EventManager.Instance.Raise(new InputToggleEvent(false));
            bad.ResetAllActions();
            current = bad;
            AIManager.Instance.ExecuteEnemyTurns(good, bad, UpdateGoodGroup);
        }
    }

    private BattleGroup UpdateGoodGroup()
    {
        return good;
    }

    private void OnChangeTurnEvent(ChangeTurnEvent e)
    {
        ChangeTurn();
    }


}
