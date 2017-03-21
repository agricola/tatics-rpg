using System;
using UnityEngine;

public class NoSelectionState : IInputState
{
    public void Enter(Character selected = null, Map map = null)
    {
        EventManager.Instance.Raise<CombatMenuEvent>(new ToggleCombatButtonsEvent(false, false, true));
    }

    public void Exit()
    {
        return;
    }

    public void OnCharacterSelect(CharacterSelectEvent e)
    {
        SetInputStateEvent ev = new SetInputStateEvent(new SelectionState(), e.character);
        EventManager.Instance.Raise(ev);
    }

    public void OnInputToggle(InputToggleEvent e)
    {
        return;
    }

    public void OnTileSelect(TileSelectEvent e)
    {
        return;
    }

    public void HandleInput()
    {
        return;
    }
}
