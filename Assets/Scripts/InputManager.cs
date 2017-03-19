using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private GameObject selected;
    [SerializeField]
    private bool inputEnabled = true;

    public GameObject Selected
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
		
	}

    private void OnTileSelect(TileSelectEvent e)
    {
        if (!selected || !inputEnabled) return;
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
        if (!inputEnabled) return;
        EventManager.Instance.Raise<PathfindEvent>(new CancelPathfindEvent());
        if (selected == e.character.gameObject)
        {
            selected = null;
            e.character.ToggleHighlight(false);
        }
        else
        {
            e.character.ToggleHighlight(true);
            selected = e.character.gameObject;
        }
    }

    private void IssueMoveCommand(Tile tile)
    {
        if (!inputEnabled) return;
        EventManager.Instance.Raise(new MoveCharacterEvent(selected.GetComponent<Character>()));
        selected.GetComponent<Character>().ToggleHighlight(false);
        selected = null;
    }

    private void IssuePathfindCommand(Tile goal)
    {
        if (!selected || !inputEnabled) return;
        Map map = GameManager.Map;
        GameObject source = map.Tiles[(int)selected.transform.localPosition.x,
            (int)selected.transform.localPosition.y].gameObject;
        int limit = source.GetComponent<Tile>().Occupant.GetComponent<Character>().MovementLimit;
        PathfindCreateEvent e = new PathfindCreateEvent(source, goal.gameObject, limit);
        EventManager.Instance.Raise<PathfindEvent>(e);
    }

    private void OnInputToggle(InputToggleEvent e)
    {
        inputEnabled = e.inputEnabled;
    }
}
