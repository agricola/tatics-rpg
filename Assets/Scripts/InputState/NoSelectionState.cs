﻿using System;
using UnityEngine;

public class NoSelectionState : IInputState
{
    public void Enter(Character selected = null, Map map = null)
    {
        //Debug.Log("no sel enter");
        EventManager.Instance.Raise<CombatMenuEvent>(new ToggleCombatButtonsEvent(false, false, true));
    }

    public void Exit()
    {
        //Debug.Log("no sel exit");
        return;
    }

    public void OnCharacterSelect(CharacterSelectEvent e)
    {
        //Debug.Log("no sel -> sel");
        if (!e.isGood) return;
        SetInputStateEvent ev = new SetInputStateEvent(new SelectionState(), e.character);
        EventManager.Instance.Raise(ev);
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