using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileChangeEvent : GameEvent
{
    public GameObject leaver;
    public TileChangeEvent(GameObject leaver)
    {
        this.leaver = leaver;
    }
}
