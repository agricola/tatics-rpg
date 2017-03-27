using System;
using System.Collections.Generic;
using System.Linq;

public class FightState : ICharacterState
{
    private Character selected;
    private List<Tile> neighbors;

    public void Enter(Character selected = null, Map map = null)
    {
        this.selected = selected;
        Map m = GameManager.Map;
        neighbors = m.GetNeighbors(selected.MapPosition);
        // EventManager.Instance.Raise<CombatMenuEvent>(new ToggleCombatButtonsEvent(false, true, true));
    }

    public void Exit()
    {
        // send out end fight event
        return;
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
        if (selected.Acted) return;
        if (e.selectType == TileSelectType.Highlight)
        {
            HandleTileHighlight(e.tile);
        }
        else if (e.selectType == TileSelectType.Move)
        {
            HandleTileClick();
        }
    }

    private void HandleTileHighlight(Tile tile)
    {
        var match = neighbors.Where(x => x == tile);
        if (!match.Contains(tile)) return;
        if (!tile.Occupant.GetComponent<Character>().IsGood)
        {
            tile.Highlight(HighlightType.Targeting);
        }
    }

    private void HandleTileClick()
    {

    }
}
