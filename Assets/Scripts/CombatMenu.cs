using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ButtonType { None, Fight, Wait, End}

public class CombatMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject combatMenu;
    [SerializeField]
    private GameObject fightButton;
    [SerializeField]
    private GameObject waitButton;
    [SerializeField]
    private GameObject endButton;

    private void Start()
	{
        if (!combatMenu) combatMenu = GameObject.Find("FightMenu");
        //ToggleMenu(false);
        EventManager.Instance.AddListener<CombatMenuEvent>(OnMenuToggle);
        ToggleButton(false, ButtonType.Fight);
        ToggleButton(false, ButtonType.Wait);
    }
	
	private void OnDestroy()
	{
        EventManager.Instance.RemoveListener<CombatMenuEvent>(OnMenuToggle);
    }

    private void OnMenuToggle(CombatMenuEvent e)
    {
        //Debug.Log("menu toggle");
        if (e is ToggleCombatMenuEvent)
        {
            ToggleCombatMenuEvent ev = e as ToggleCombatMenuEvent;
            ToggleMenu(ev.menuOn);
        }
        else if (e is ToggleCombatButtonsEvent)
        {
            ToggleCombatButtonsEvent ev = e as ToggleCombatButtonsEvent;
            ToggleButton(ev.fightOn, ButtonType.Fight);
            ToggleButton(ev.waitOn, ButtonType.Wait);
            ToggleButton(ev.endOn, ButtonType.End);
        }
        //Debug.Log("menu toggle finish");
    }

    private void ToggleMenu(bool menuOn)
    {
        if (combatMenu) combatMenu.SetActive(menuOn);
    }

    private void ToggleButton(bool buttonOn, ButtonType button)
    {
        switch (button)
        {
            case ButtonType.None:
                break;
            case ButtonType.Fight:
                fightButton.SetActive(buttonOn);
                break;
            case ButtonType.Wait:
                waitButton.SetActive(buttonOn);
                break;
            case ButtonType.End:
                endButton.SetActive(buttonOn);
                break;
            default:
                break;
        }
    }
}
