using System;

public class ActionState : ICharacterState
{
    public void Enter(Character selected = null, Map map = null)
    {
        EventManager.Instance.Raise<CombatMenuEvent>(new ToggleCombatMenuEvent(true));
        //EventManager.Instance.Raise<CombatMenuEvent>(new ToggleCombatButtonsEvent(true, true, true));
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
