using System;
using UnityEngine;

public class MoveState : ICharacterState
{
    private Character selected;

    public void Enter(Character selected = null, Map map = null)
    {
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
        if (selected.Moved) return;
        if (e.selectType == TileSelectType.Highlight)
        {
            IssuePathfindCommand(e.tile);
        }
        else if (e.selectType == TileSelectType.Move)
        {
            IssueMoveCommand(e.tile);
        }
    }

    private void EndPathfinding()
    {
        EventManager.Instance.Raise<PathfindEvent>(new CancelPathfindEvent());
    }

    private void IssueMoveCommand(Tile tile)
    {
        bool skip = tile.Occupant == selected.gameObject ? true : false;
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
