using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharacterEvent : GameEvent
{
    public Character Character { get; private set; }
    public bool Skip { get; private set; }
    public MoveCharacterEvent(Character character, bool skip)
    {
        Character = character;
        Skip = skip;
    }
}
