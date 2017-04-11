using System.Collections.Generic;

public enum HighlightSelection { Main, Sub }
public class HighlightEvent : GameEvent
{
    public HighlightSelection Selection { get; private set; }
    public List<Tile> Tiles { get; private set; }
    public HighlightEvent(HighlightSelection selection, List<Tile> tiles = null)
    {
        Selection = selection;
        Tiles = tiles;
    }
}
/* 
public class UpdateTilesEvent : HighlightEvent
{
    public List<Tile> Tiles { get; private set;}
    public UpdateTilesEvent(HighlightSelection selection, List<Tile> tiles, bool isOn = false)
        : base(selection, isOn)
    {
        Tiles = tiles;
    }
}

public class ClearTilesEvent : HighlightEvent
{
    public ClearTilesEvent() : base(HighlightSelection.Main)
    {
        
    }
}*/