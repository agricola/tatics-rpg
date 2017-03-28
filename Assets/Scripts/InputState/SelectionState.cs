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
        EventManager.Instance.AddListener<AnimationEvent>(OnAnim);
    }

    public void Exit()
    {
        SetCombatMenu();
        if (characterState != null) characterState.Exit();
        if (selected) selected.ToggleHighlight(false);
        EventManager.Instance.RemoveListener<AnimationEvent>(OnAnim);
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

    private void OnAnim(AnimationEvent e)
    {
        if ((characterState is MoveState || characterState is FightState) && !e.Act && e.Actor == selected.gameObject)
        {
            if (e is ToggleWalkEvent)
            {
                TransitionCharacterState(new ActionState());
            }
            else if (e is ToggleFightEvent)
            {
                TransitionToNoSelection();
            }
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
