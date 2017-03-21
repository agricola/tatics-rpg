using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private Character selected;
    [SerializeField]
    private bool inputEnabled = true;
    [SerializeField]
    private bool lockSelected = false;
    [SerializeField]
    private IInputState state;

    public Character Selected
    {
        get
        {
            return selected;
        }
        set
        {
            selected = value;
        }
    }

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
        this.state.Exit();
        this.state = state;
        this.state.Enter(enterObj);
    }

    private void OnTileSelect(TileSelectEvent e)
    {
        //if (!inputEnabled) return;
        state.OnTileSelect(e);
    }

    private void OnCharacterSelect(CharacterSelectEvent e)
    {
        if (e.character.Moved) return;
        state.OnCharacterSelect(e);
    }

    private void OnInputToggle(InputToggleEvent e)
    {
        //inputEnabled = e.inputEnabled;
        if (e.inputEnabled)
        {
            ChangeState(new NoSelectionState());
        }
        else
        {
            ChangeState(new NoInputState());
        }
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
        ChangeState(new NoSelectionState());
    }

    public void PressEndButton()
    {
        //ChangeState(new NoInputState());
        EventManager.Instance.Raise(new EndTurnEvent());
    }
}
