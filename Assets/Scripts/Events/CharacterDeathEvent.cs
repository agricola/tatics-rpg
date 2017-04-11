﻿using UnityEngine;

public class CharacterDeathEvent : GameEvent
{
    public GameObject DeadCharacter { get; private set; }
    public CharacterDeathEvent(GameObject deadCharacter)
    {
        DeadCharacter = deadCharacter;
    }

}
