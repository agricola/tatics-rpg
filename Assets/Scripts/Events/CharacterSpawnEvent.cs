using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterChangeEvent : GameEvent
{
    public Character character;
    public bool create;

    public CharacterChangeEvent(Character character, bool create)
    {
        this.character = character;
        this.create = create;
    }
}
