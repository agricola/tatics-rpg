using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharacterEvent : GameEvent
{
    public Character character;
    public MoveCharacterEvent(Character character)
    {
        this.character = character;
    }
}
