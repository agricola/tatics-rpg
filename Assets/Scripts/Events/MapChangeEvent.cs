public class MapChangeEvent : GameEvent
{
    public Map map;
    public MapChangeEvent(Map map)
    {
        this.map = map;
    }
}
