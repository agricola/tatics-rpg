using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private bool inputEnabled = true;
    private IInputState state;

	private void Start()
	{
        if (state != null) state.Exit();
        state = new NoSelectionState();
        state.Enter();
    }

    private void OnEnable()
    {
        EventManager em = EventManager.Instance;
        if (em)
        {
            EventManager.Instance.AddListener<TileSelectEvent>(OnTileSelect);
            EventManager.Instance.AddListener<CharacterSelectEvent>(OnCharacterSelect);
            EventManager.Instance.AddListener<InputToggleEvent>(OnInputToggle);
            EventManager.Instance.AddListener<SetInputStateEvent>(OnInputStateChange);
            EventManager.Instance.AddListener<AnimationEvent>(OnAnimationEvent);
        }
    }

    private void OnDisable()
    {
        //if (state != null) state.Exit();
        EventManager em = EventManager.Instance;
        if (em)
        {
            em.RemoveListener<TileSelectEvent>(OnTileSelect);
            em.RemoveListener<CharacterSelectEvent>(OnCharacterSelect);
            em.RemoveListener<InputToggleEvent>(OnInputToggle);
            em.RemoveListener<SetInputStateEvent>(OnInputStateChange);
            em.RemoveListener<AnimationEvent>(OnAnimationEvent);
        }
    }

    private void Update()
    {
        //if (state == null) state = new NoSelectionState();
        if (!inputEnabled) return;
        state.HandleInput();
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            EventManager.Instance.Raise(new OptionsMenuEvent(true));
        }
    }

    private void OnInputStateChange(SetInputStateEvent e)
    {
        ChangeState(e.state, e.enterObj);
    }

    private void ChangeState(IInputState state, Character enterObj = null)
    {
        if (this.state != null ) this.state.Exit();
        this.state = state;
        this.state.Enter(enterObj);
    }

    private void OnTileSelect(TileSelectEvent e)
    {
        if (!inputEnabled) return;
        state.OnTileSelect(e);
    }

    private void OnCharacterSelect(CharacterSelectEvent e)
    {
        if (!inputEnabled) return;
        state.OnCharacterSelect(e);
    }

    private void OnInputToggle(InputToggleEvent e)
    {
        ToggleInput(e.inputEnabled);
    }

    private void ToggleInput(bool enabled)
    {
        inputEnabled = enabled;
        EventManager.Instance.Raise(new CombatMenuEvent());
    }

    public void PressWaitButton()
    {
        if (state is SelectionState)
        {
            SelectionState s = state as SelectionState;
            s.Selected.Acted = true;
        }
        TransitionToNoSelectionState();
    }

    public void PressEndButton()
    {
        //ChangeState(new NoInputState());
        TransitionToNoSelectionState();
        ToggleInput(false);
        EventManager.Instance.Raise(new EndTurnEvent());
    }

    public void PressFightButton()
    {
        if (state is SelectionState)
        {
            SelectionState s = state as SelectionState;
            s.TransitionToFightState();
        }
        // add fight finish listener
        //TransitionToNoSelectionState();
    }

    public void PressSpecialButton()
    {/*
        if (state is SelectionState)
        {
            SelectionState s = state as SelectionState;
            s.TransitionToFightState();
        }*/
        ICharacterState s = new LineTargetState();
        EventManager.Instance.Raise(new CharacterStateTransitionEvent(s));
        // add fight finish listener
        //TransitionToNoSelectionState();
    }

    private void TransitionToNoSelectionState()
    {
        ChangeState(new NoSelectionState());
    }
    private void OnAnimationEvent(AnimationEvent e)
    {
        if (e is AnimationWalkEvent &&
            e.Status == AnimationStatus.Start &&
            e.Actor.GetComponent<Character>().IsGood)
        {
            ToggleInput(false);
        }
    }

}
