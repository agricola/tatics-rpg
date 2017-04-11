using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionState : IInputState
{
    private Character selected;
    private ICharacterState characterState;
    private bool exitable = false;

    public Character Selected
    {
        get
        {
            return selected;
        }
    }

    public void Enter(Character selected = null, Map map = null)
    {
        EventManager.Instance.Raise(new CombatMenuEvent());
        if (this.selected == null)
        {
            this.selected = selected;
            this.selected.ToggleHighlight(true);
            ICharacterState state = (selected.Moved || selected.Acted) ? new ActionState() : new MoveState() as ICharacterState;
            TransitionCharacterState(state);
        }
        EventManager.Instance.AddListener<CharacterStateTransitionEvent>(OnCharStateTransition);
        EventManager.Instance.Raise(new ColliderToggleEvent(false));
    }

    private void OnCharStateTransition(CharacterStateTransitionEvent e)
    {
        TransitionCharacterState(e.CharacterState);
    }

    public void Exit()
    {
        SetCombatMenu();
        if (characterState != null) characterState.Exit();
        if (selected) selected.ToggleHighlight(false);
        EventManager em = EventManager.Instance;
        if (em)
        {
            em.RemoveListener<CharacterStateTransitionEvent>(OnCharStateTransition);
        }
        EventManager.Instance.Raise(new ColliderToggleEvent(true));
    }

    public void OnCharacterSelect(CharacterSelectEvent e)
    {
        if (!e.isGood) return;
        characterState.OnCharacterSelect(e);
        SelectCharacter(e.character);
    }

    public void OnTileSelect(TileSelectEvent e)
    {/*
        if (e.selectType == TileSelectType.Cancel)
        {
            TransitionToNoSelection();
            return;
        }*/
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
    {
        if (!exitable)
        {
            exitable = true;
            return;
        }
        if (!exitable) return;
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            TransitionToNoSelection();
        }
        characterState.HandleInput();
    }

    private void TransitionCharacterState(ICharacterState state)
    {
        if (characterState != null) characterState.Exit();
        characterState = state;
        characterState.Enter(selected);
        //SetCombatMenu();
    }

    public void TransitionToFightState()
    {
        TransitionCharacterState(new FightState());
    }

    private void SetCombatMenu()
    {
        EventManager.Instance.Raise(new CombatMenuEvent(!selected.Acted, !selected.Acted));
    }
}
