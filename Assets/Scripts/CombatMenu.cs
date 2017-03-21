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
        ToggleMenu(false);
        EventManager.Instance.AddListener<ToggleCombatMenuEvent>(OnMenuToggle);
	}
	
	private void OnDestroy()
	{
        EventManager.Instance.RemoveListener<ToggleCombatMenuEvent>(OnMenuToggle);
    }

    private void OnMenuToggle(ToggleCombatMenuEvent e)
    {
        ToggleMenu(e.menuOn);
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
