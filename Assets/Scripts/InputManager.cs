using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeselectEvent : GameEvent { }

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private GameObject selected;
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
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener<TileSelectEvent>(OnTileSelect);
        EventManager.Instance.AddListener<CharacterSelectEvent>(OnCharacterSelect);
    }
	
	private void Update()
	{
		
	}

    private void OnTileSelect(TileSelectEvent e)
    {
        if (!selected) return;
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
        selected = e.character.gameObject;
    }

    private void IssueMoveCommand(Tile tile)
    {
        Debug.Log(tile.transform.localPosition + " MOVE COMMAND");
        EventManager.Instance.Raise<DeselectEvent>(new DeselectEvent());
        selected = null;
    }

    private void IssuePathfindCommand(Tile goal)
    {
        if (!selected)
        {
            Debug.Log("nothing selected! xD");
            return;
        }
        GameObject map = GameObject.Find("Map");
        GameObject source = map.GetComponent<Map>()
            .Tiles[(int)selected.transform.localPosition.x,
            (int)selected.transform.localPosition.y];
        PathfindCreateEvent e = new PathfindCreateEvent(source, goal.gameObject);
        EventManager.Instance.Raise<PathfindEvent>(e);
    }
}
