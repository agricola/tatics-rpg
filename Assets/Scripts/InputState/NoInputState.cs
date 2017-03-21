using System;
using UnityEngine;

public class NoInputState : IInputState
{
    public void Enter(Character selected = null, Map map = null)
    {
        return;
    }

    public void Exit()
    {
        return;
    }

    public void OnCharacterSelect(CharacterSelectEvent e)
    {
        return;
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
