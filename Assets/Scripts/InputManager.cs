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
        EventManager.Instance.AddListener<TileSelectEvent>(OnTileSelect);
        EventManager.Instance.AddListener<CharacterSelectEvent>(OnCharacterSelect);
        EventManager.Instance.AddListener<InputToggleEvent>(OnInputToggle);
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener<TileSelectEvent>(OnTileSelect);
        EventManager.Instance.AddListener<CharacterSelectEvent>(OnCharacterSelect);
        EventManager.Instance.RemoveListener<InputToggleEvent>(OnInputToggle);
    }
	
	private void Update()
	{
        if (!inputEnabled) return;
		if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("end turn");
            EventManager.Instance.Raise(new EndTurnEvent());
            
        }
	}

    private void OnTileSelect(TileSelectEvent e)
    {
        if (!selected || !inputEnabled || selected.Moved) return;
        if (e.selectType == TileSelectType.Highlight)
        {
            IssuePathfindCommand(e.tile);
        }
        else if (e.selectType == TileSelectType.Move)
        {
            IssueMoveCommand(e.tile);
        }
    }

    private void OnCharacterSelect(CharacterSelectEvent e)
    {
        if (!inputEnabled || e.character.Moved || !e.character.IsGood) return;
        if (selected == e.character)
        {
            EndPathfinding();
            selected = null;
            EventManager.Instance.Raise<CombatMenuEvent>(new ToggleCombatButtonsEvent(false, false, true));
            e.character.ToggleHighlight(false);
            EventManager.Instance.Raise<RadiusEvent>(new DestroyRadiusEvent());
            lockSelected = false;
        }
        else
        {
            if (lockSelected) return;
            EndPathfinding();
            EventManager.Instance.Raise<CombatMenuEvent>(new ToggleCombatButtonsEvent(true, true, true));
            e.character.ToggleHighlight(true);
            selected = e.character;
            EventManager.Instance.Raise<RadiusEvent>(new CreateRadiusEvent(e.character));
            lockSelected = true;
        }
    }

    private void EndPathfinding()
    {
        EventManager.Instance.Raise<PathfindEvent>(new CancelPathfindEvent());
        EventManager.Instance.Raise(new UnhighlightTilesEvent());
    }

    private void IssueMoveCommand(Tile tile)
    {
        if (!inputEnabled) return;
        EventManager.Instance.Raise(new MoveCharacterEvent(selected));
        selected.ToggleHighlight(false);
        selected.Moved = true;
        EventManager.Instance.Raise<CombatMenuEvent>(new ToggleCombatButtonsEvent(true, true, true));
    }

    private void IssuePathfindCommand(Tile goal)
    {
        if (!selected || !inputEnabled) return;
        Map map = GameManager.Map;
        Tile source = map.Tiles[(int)selected.transform.localPosition.x,
            (int)selected.transform.localPosition.y];
        int limit = source.GetComponent<Tile>().Occupant.GetComponent<Character>().MovementLimit;
        PathfindCreateEvent e = new PathfindCreateEvent(source, goal, limit);
        EventManager.Instance.Raise<PathfindEvent>(e);
    }

    private void OnInputToggle(InputToggleEvent e)
    {
        inputEnabled = e.inputEnabled;
    }

    private void FinishSelected()
    {
        selected.Acted = true;
        lockSelected = false;
        selected = null;
    }

    public void PressWaitButton()
    {
        FinishSelected();
        EventManager.Instance.Raise<CombatMenuEvent>(new ToggleCombatButtonsEvent(false, false, true));
    }

    public void PressEndButton()
    {
        selected = null;
        lockSelected = false;
        EventManager.Instance.Raise(new EndTurnEvent());
    }
}
