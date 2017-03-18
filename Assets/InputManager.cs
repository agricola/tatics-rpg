using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputEvent : GameEvent
{

}

public class InputManager : MonoBehaviour
{

	private void Awake()
	{
        EventManager.Instance.AddListener<TileEvent>(ReadTileEvent);
	}

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener<TileEvent>(ReadTileEvent);
    }
	
	private void Update()
	{
		
	}

    private void ReadTileEvent(TileEvent e)
    {
        if (e.highlighted)
        {
            
        }
        else
        {
            //IssueMoveCommand(e.tile);
            IssuePathfindCommand(e.tile); // temp here
        }
    }

    private void IssueMoveCommand(Tile tile)
    {
        Debug.Log(tile.transform.localPosition);
    }

    private void IssuePathfindCommand(Tile goal)
    {
        GameObject player = GameObject.Find("Player(Clone)");
        GameObject map = GameObject.Find("Map");
        Debug.Log(player);
        GameObject source = map.GetComponent<Map>()
            .Tiles[(int)player.transform.localPosition.x, (int)player.transform.localPosition.y];
        GameObject.Find("CommandManager").GetComponent<CommandManager>().FindPath(source, goal.gameObject);
    }
}
