using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class MapChangeEvent : GameEvent
{
    public Map map;
    public MapChangeEvent(Map map)
    {
        this.map = map;
    }
}
