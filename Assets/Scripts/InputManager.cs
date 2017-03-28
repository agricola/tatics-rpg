using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private bool inputEnabled = true;
    private IInputState state;

	private void Start()
	{
        if (state != null) state.Exit();
        state = new NoSelectionState();
        state.Enter();
        EventManager.Instance.AddListener<TileSelectEvent>(OnTileSelect);
        EventManager.Instance.AddListener<CharacterSelectEvent>(OnCharacterSelect);
        EventManager.Instance.AddListener<InputToggleEvent>(OnInputToggle);
        EventManager.Instance.AddListener<SetInputStateEvent>(OnInputStateChange);
    }

    private void OnApplicationQuit()
    {
        Debug.Log("quit successfully!");
    }

    private void OnDestroy()
    {
        //if (state != null) state.Exit();
        EventManager.Instance.RemoveListener<TileSelectEvent>(OnTileSelect);
        EventManager.Instance.RemoveListener<CharacterSelectEvent>(OnCharacterSelect);
        EventManager.Instance.RemoveListener<InputToggleEvent>(OnInputToggle);
        EventManager.Instance.RemoveListener<SetInputStateEvent>(OnInputStateChange);
    }

    private void Update()
    {
        //if (state == null) state = new NoSelectionState();
        state.HandleInput();
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
        //Debug.Log("toogle");
        inputEnabled = e.inputEnabled;
        EventManager.Instance.Raise<CombatMenuEvent>(new ToggleCombatMenuEvent(inputEnabled));
        //Debug.Log(state);
    }

    public void PressWaitButton()
    {
        //Debug.Log("wait");
        if (state is SelectionState)
        {
            SelectionState s = state as SelectionState;
            s.Selected.Acted = true;
        }
        TransitionToNoSelectionState();
        //Debug.Log("wait 2");
    }

    public void PressEndButton()
    {
        //ChangeState(new NoInputState());
        TransitionToNoSelectionState();
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
        //Debug.Log("wait 2");
    }

    private void TransitionToNoSelectionState()
    {
        ChangeState(new NoSelectionState());
    }
}
