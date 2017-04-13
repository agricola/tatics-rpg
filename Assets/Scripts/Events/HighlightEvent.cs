using System.Collections.Generic;

public enum HighlightSelection { Main, Sub }


/// <summary>
/// Event to highlight certain tiles</summary>
public class HighlightEvent : GameEvent
{
    public HighlightSelection Selection { get; private set; }
    public List<Tile> Tiles { get; private set; }

    /// <summary>
    /// Sets tiles to highlight. If no tiles, will clear highlight. </summary>
    /// <param name="selection"> Sub or Main, the type of selection</param>
    /// <param name="tiles"> The tiles you want to highlight. Null by default</param>
    public HighlightEvent(HighlightSelection selection, List<Tile> tiles = null)
    {
        Selection = selection;
        Tiles = tiles;
    }
}