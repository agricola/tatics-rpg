public class SelectLevelEvent : GameEvent
{
    public int SelectedLevel { get; private set; }
    public SelectLevelEvent(int selectedLevel)
    {
        SelectedLevel = selectedLevel;
    }
}