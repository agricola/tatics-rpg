using UnityEngine;

public interface IInputState
{
    void OnTileSelect(TileSelectEvent e);
    void OnCharacterSelect(CharacterSelectEvent e);
    void Enter(Character selected = null, Map map = null);
    void Exit();
    void HandleInput();
}
