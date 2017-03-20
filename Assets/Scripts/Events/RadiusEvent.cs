using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class RadiusEvent : GameEvent { }

public class CreateRadiusEvent : RadiusEvent
{
    public Character character;
    public CreateRadiusEvent(Character character)
    {
        this.character = character;
    }
}

public class DestroyRadiusEvent : RadiusEvent { }
