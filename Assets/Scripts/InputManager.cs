using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private bool inputEnabled = true;
    private IInputState state;

	private void Awake()
	{
        if (state != null) state.Exit();
        state = new NoSelectionState();
        state.Enter();
        EventManager.Instance.AddListener<TileSelectEvent>(OnTileSelect);
        EventManager.Instance.AddListener<CharacterSelectEvent>(OnCharacterSelect);
        EventManager.Instance.AddListener<InputToggleEvent>(OnInputToggle);
        EventManager.Instance.AddListener<SetInputStateEvent>(OnInputStateChange);
    }

    private void OnDestroy()
    {
        if (state != null) state.Exit();
        EventManager.Instance.RemoveListener<TileSelectEvent>(OnTileSelect);
        EventManager.Instance.RemoveListener<CharacterSelectEvent>(OnCharacterSelect);
        EventManager.Instance.RemoveListener<InputToggleEvent>(OnInputToggle);
        EventManager.Instance.RemoveListener<SetInputStateEvent>(OnInputStateChange);
    }

    private void Update()
    {
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
    /*
    private void FinishSelected()
    {
        selected.Acted = true;
        lockSelected = false;
        selected = null;
    }*/

    public void PressWaitButton()
    {
        //Debug.Log("wait");
        ChangeState(new NoSelectionState());
        //Debug.Log("wait 2");
    }

    public void PressEndButton()
    {
        //ChangeState(new NoInputState());
        ChangeState(new NoSelectionState());
        EventManager.Instance.Raise(new EndTurnEvent());
    }
}
