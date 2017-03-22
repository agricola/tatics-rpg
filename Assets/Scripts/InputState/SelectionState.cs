using System;
using UnityEngine;

public class SelectionState : IInputState
{
    private Character selected;
    private ICharacterState characterState;
    private bool exitable = false;

    public SelectionState()
    {
        EventManager.Instance.AddListener<ToggleWalkEvent>(OnWalk);
    }

    ~SelectionState()
    {
        EventManager.Instance.RemoveListener<ToggleWalkEvent>(OnWalk);
    }

    public void Enter(Character selected = null, Map map = null)
    {
        //Debug.Log("sel enter");
        EventManager.Instance.Raise<CombatMenuEvent>(new ToggleCombatButtonsEvent(true, true, true));
        if (this.selected == null)
        {
            this.selected = selected;
            this.selected.ToggleHighlight(true);
            characterState = this.selected.Moved ? new ActionState() as ICharacterState : new MoveState();
            characterState.Enter(selected);
        }
    }

    public void Exit()
    {
        //Debug.Log("sel exit");
        if (characterState != null) characterState.Exit();
        if (selected) selected.ToggleHighlight(false);
    }

    public void OnCharacterSelect(CharacterSelectEvent e)
    {
        if (!e.isGood) return;
        characterState.OnCharacterSelect(e);
        SelectCharacter(e.character);
    }

    public void OnTileSelect(TileSelectEvent e)
    {
        if (e.selectType == TileSelectType.Cancel)
        {
            TransitionToNoSelection();
            return;
        }
        characterState.OnTileSelect(e);
    }

    private void FinishSelected()
    {
        selected.Acted = true;
        TransitionToNoSelection();
    }

    private void SelectCharacter(Character c)
    {
        if (selected == c)
        {
            TransitionToNoSelection();
        }
        else
        {
            SelectionState s = new SelectionState();
            EventManager.Instance.Raise(new SetInputStateEvent(s, c));
        }
    }

    private void TransitionToNoSelection()
    {
        NoSelectionState s = new NoSelectionState();
        EventManager.Instance.Raise(new SetInputStateEvent(s));
    }

    public void HandleInput()
    {/*
        if (!exitable)
        {
            exitable = true;
            return;
        }
        characterState.HandleInput();
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            TransitionToNoSelection();
        }*/
    }

    private void OnWalk(ToggleWalkEvent e)
    {
        if (characterState is MoveState && !e.walk && e.walker == selected.gameObject)
        {
            TransitionToActionState();
            //Debug.Log("woop");
        }
    }

    private void TransitionToActionState()
    {
        characterState.Exit();
        characterState = new ActionState();
        characterState.Enter();
    }
}
