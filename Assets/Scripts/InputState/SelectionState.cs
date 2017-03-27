using System;
using UnityEngine;

public class SelectionState : IInputState
{
    private Character selected;
    private ICharacterState characterState;

    public Character Selected
    {
        get
        {
            return selected;
        }
    }

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
        EventManager.Instance.Raise< CombatMenuEvent>(new ToggleCombatMenuEvent(false));
        if (this.selected == null)
        {
            this.selected = selected;
            this.selected.ToggleHighlight(true);
            ICharacterState state = (selected.Moved || selected.Acted) ? new ActionState() : new MoveState() as ICharacterState;
            TransitionCharacterState(state);
        }
    }

    public void Exit()
    {
        SetCombatMenu();
        if (characterState != null) characterState.Exit();
        selected.ToggleHighlight(false);
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
            TransitionCharacterState(new ActionState());
            //Debug.Log("woop");
        }
    }

    private void TransitionCharacterState(ICharacterState state)
    {
        if (characterState != null) characterState.Exit();
        characterState = state;
        characterState.Enter(selected);
        SetCombatMenu();
    }

    public void TransitionToFightState()
    {
        TransitionCharacterState(new FightState());
    }

    private void SetCombatMenu()
    {
        EventManager.Instance.Raise<CombatMenuEvent>(
            new ToggleCombatButtonsEvent(!selected.Acted, !selected.Acted, true));
    }
}
