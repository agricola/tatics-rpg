using System;
using UnityEngine;

public class MoveState : ICharacterState
{
    private Character selected;

    public void Enter(Character selected = null, Map map = null)
    {
        //Debug.Log("move enter");
        this.selected = selected;
        EventManager.Instance.Raise<RadiusEvent>(new CreateRadiusEvent(selected));
        EventManager.Instance.Raise(new CombatMenuEvent());
    }

    public void Exit()
    {
        EndPathfinding();
    }

    public void HandleInput()
    {
        return;
    }

    public void OnCharacterSelect(CharacterSelectEvent e)
    {
        return;
    }

    public void OnTileSelect(TileSelectEvent e)
    {
        //Debug.Log("move tile sel 0");
        if (selected.Moved) return;
        if (e.selectType == TileSelectType.Highlight)
        {
            //Debug.Log("move tile sel 1");
            IssuePathfindCommand(e.tile);
        }
        else if (e.selectType == TileSelectType.Move)
        {
            //Debug.Log("move tile sel 2");
            IssueMoveCommand(e.tile);
        }
        //Debug.Log("move tile sel 3");
    }

    private void EndPathfinding()
    {
        Debug.Log("end pathfinding");
        EventManager.Instance.Raise<PathfindEvent>(new CancelPathfindEvent());
    }

    private void IssueMoveCommand(Tile tile)
    {
        //selected.Moved = true;
        bool skip = tile.Occupant == selected.gameObject ? true : false;
        Debug.Log(skip);
        EventManager.Instance.Raise(new MoveCharacterEvent(selected, skip));
    }

    private void IssuePathfindCommand(Tile goal)
    {
        //if (!selected || !inputEnabled) return;
        Map map = GameManager.Map;
        Tile source = map.Tiles[(int)selected.transform.localPosition.x,
            (int)selected.transform.localPosition.y];
        int limit = source.GetComponent<Tile>().Occupant.GetComponent<Character>().MovementLimit;
        PathfindCreateEvent e = new PathfindCreateEvent(source, goal, limit);
        EventManager.Instance.Raise<PathfindEvent>(e);
    }
}
