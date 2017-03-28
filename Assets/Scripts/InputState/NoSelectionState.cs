using System;
using UnityEngine;

public class NoSelectionState : IInputState
{
    public void Enter(Character selected = null, Map map = null)
    {
        EventManager.Instance.Raise(new CombatMenuEvent());
    }

    public void Exit()
    {
        return;
    }

    public void OnCharacterSelect(CharacterSelectEvent e)
    {
        if (!e.isGood) return;
        EventManager em = EventManager.Instance;
        if (em)
        {
            SetInputStateEvent ev = new SetInputStateEvent(new SelectionState(), e.character);
            em.Raise(ev);
        }
    }

    public void OnTileSelect(TileSelectEvent e)
    {
        return;
    }

    public void HandleInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            EventManager.Instance.Raise(new OptionsMenuEvent());
        }
    }
}
