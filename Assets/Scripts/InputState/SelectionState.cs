using System;
using UnityEngine;

public class SelectionState : IInputState
{
    private Character selected;
    private IInputState characterState;
    private bool exitable = false;

    public void Enter(Character selected = null, Map map = null)
    {
        EventManager.Instance.Raise<CombatMenuEvent>(new ToggleCombatButtonsEvent(true, true, true));
        SelectCharacter(selected);
        characterState = new MoveState();
        characterState.Enter(selected);
    }

    public void Exit()
    {
        Debug.Log("exit");
        if (characterState != null) characterState.Exit();
        if (selected) selected.ToggleHighlight(false);
    }

    public void OnCharacterSelect(CharacterSelectEvent e)
    {
        characterState.OnCharacterSelect(e);
        if (e.character != selected)
        {
            SelectCharacter(e.character);
        }
        else
        {
            TransitionToNoSelection();
        }
    }

    public void OnInputToggle(InputToggleEvent e)
    {
        characterState.OnInputToggle(e);
        NoInputState s = new NoInputState();
        EventManager.Instance.Raise(new SetInputStateEvent(s));
    }

    public void OnTileSelect(TileSelectEvent e)
    {
        characterState.OnTileSelect(e);
    }

    private void FinishSelected()
    {
        selected.Acted = true;
        TransitionToNoSelection();
    }

    private void SelectCharacter(Character c)
    {
        if (selected) selected.ToggleHighlight(false);
        selected = c;
        c.ToggleHighlight(true);
    }

    private void TransitionToNoSelection()
    {
        NoSelectionState s = new NoSelectionState();
        EventManager.Instance.Raise(new SetInputStateEvent(s));
    }

    public void HandleInput()
    {
        if (!exitable)
        {
            exitable = true;
            return;
        }
        characterState.HandleInput();
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            TransitionToNoSelection();
        }
    }
}
