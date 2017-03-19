public enum TileSelectType { None, Highlight, Move }

public class TileSelectEvent : GameEvent
{
    public TileSelectType selectType;
    public Tile tile;
    public TileSelectEvent(Tile tile, TileSelectType selectType)
    {
        this.tile = tile;
        this.selectType = selectType;
    }
}