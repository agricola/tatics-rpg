using System.Collections.Generic;

public class FinishGeneratingMapsEvent : GameEvent
{
    public List<MapTiles> Maps { get; private set; }
    public FinishGeneratingMapsEvent(List<MapTiles> maps)
    {
        Maps = maps;
    }
}