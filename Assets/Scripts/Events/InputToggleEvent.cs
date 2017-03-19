using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class InputToggleEvent : GameEvent
{
    public bool inputEnabled;
    public InputToggleEvent(bool inputEnabled)
    {
        this.inputEnabled = inputEnabled;
    }
}
