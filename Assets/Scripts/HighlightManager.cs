using System.Collections.Generic;
using UnityEngine;
public class HighlightManager : MonoBehaviour
{
    [SerializeField]
    private Color neutralColor = Color.white;
    [SerializeField]
    private Color subColor = Color.yellow;
    [SerializeField]
    private Color mainColor = Color.red;
    private List<Tile> mainSelection;
    private List<Tile> subSelection;

    void OnEnable()
    {
        EventManager em = EventManager.Instance;
        if (em)
        {
            em.AddListener<HighlightEvent>(OnHighlightEvent);
        }
    }

    void OnDisable()
    {
        EventManager em = EventManager.Instance;
        if (em)
        {
            em.RemoveListener<HighlightEvent>(OnHighlightEvent);
        }
    }

    private void OnHighlightEvent(HighlightEvent e)
    {
        if (e.Selection == HighlightSelection.Main)
        {
            ToggleHighlight(false, HighlightSelection.Main);
            mainSelection = e.Tiles;
            if (mainSelection != null) ToggleHighlight(true, HighlightSelection.Main);
        }
        else
        {
            ToggleHighlight(false, HighlightSelection.Sub);
            ToggleHighlight(true, HighlightSelection.Main);
            subSelection = e.Tiles;
            if (subSelection != null) ToggleHighlight(true, HighlightSelection.Sub);
        }
    }

    private void ToggleHighlight(bool on, HighlightSelection sel)
    {
        Color color = neutralColor;
        List<Tile> tiles = new List<Tile>();
        switch (sel)
        {
            case HighlightSelection.Main:
                color = on ? mainColor : neutralColor;
                tiles = mainSelection;
                ToggleHighlight(false, HighlightSelection.Sub);
                break;
            case HighlightSelection.Sub:
                color = on ? subColor : neutralColor;
                tiles = subSelection;
                break;
            default:
                break;
        }
        HighlightTiles(tiles, color);
    }

    private void HighlightTiles(List<Tile> tiles, Color color)
    {
        if (tiles == null || tiles.Count <= 0) return;
        tiles.ForEach(x => x.Highlight(color));
    }
}
