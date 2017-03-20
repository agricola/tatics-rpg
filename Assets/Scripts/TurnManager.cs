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
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener<EndTurnEvent>(OnEndTurn);
        EventManager.Instance.RemoveListener<SetBattleGroupsEvent>(OnSetBattleGroups);
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
        if (current == bad)
        {
            EventManager.Instance.Raise(new InputToggleEvent(true));
            good.ResetAllActions();
            turnNumber++;
            current = good;
            Debug.Log("good turn");
        }
        else
        {
            EventManager.Instance.Raise(new InputToggleEvent(false));
            bad.ResetAllActions();
            current = bad;
            Debug.Log("bad turn");
            StartCoroutine(EvilScheming());
        }
    }

    private IEnumerator EvilScheming()
    {
        Debug.Log("evil scheming...");
        yield return new WaitForSeconds(5);
        EventManager.Instance.Raise(new EndTurnEvent());
    }
}
