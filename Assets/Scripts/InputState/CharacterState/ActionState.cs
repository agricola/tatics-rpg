using System;

public class ActionState : ICharacterState
{
    public void Enter(Character selected = null, Map map = null)
    {
        EventManager.Instance.Raise(new CombatMenuEvent(!selected.Acted, !selected.Acted));
        //EventManager.Instance.Raise(new CombatMenuEvent(true, true, true));
    }

    public void Exit()
    {
        return;
    }

    public void HandleInput()
    {
        return;
    }

    public void OnCharacterSelect(CharacterSelectEvent e)
    {
        return;
    }

    public void OnTileSelect(TileSelectEvent e)
    {
        return;
    }
}
