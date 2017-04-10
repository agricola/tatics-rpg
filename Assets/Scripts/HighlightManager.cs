using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        if (e is UpdateTilesEvent)
        {
            OnUpdateTilesEvent(e as UpdateTilesEvent);
        }
        else if (e is ClearTilesEvent)
        {
            OnClearTilesEvent(e as ClearTilesEvent);
        }
        else
        {
            OnToggleHighlightEvent(e);
        }
    }

    private void OnClearTilesEvent(ClearTilesEvent e)
    {
        //Debug.Log("clear");
        ToggleHighlight(false, HighlightSelection.Main);
        //ToggleHighlight(false, HighlightSelection.Sub);
        mainSelection = null;
        subSelection = null;
    }

    private void OnUpdateTilesEvent(UpdateTilesEvent e)
    {
        Debug.Log("update " + e.Selection + " " + e.IsOn + e.Tiles.Count);
        switch (e.Selection)
        {
            case HighlightSelection.Main:
                mainSelection = e.Tiles;
                break;
            case HighlightSelection.Sub:
                subSelection = e.Tiles;
                break;
            default:
                break;
        }
        ToggleHighlight(e.IsOn, e.Selection);
    }

    private void OnToggleHighlightEvent(HighlightEvent e)
    {
        Debug.Log("toggle " + e.Selection + " " + e.IsOn);
        ToggleHighlight(e.IsOn, e.Selection);
    }

    private void ToggleHighlight(bool isOn, HighlightSelection sel)
    {
        Color color = neutralColor;
        List<Tile> tiles = new List<Tile>();
        switch (sel)
        {
            case HighlightSelection.Main:
                color = isOn ? mainColor : neutralColor;
                tiles = mainSelection;
                break;
            case HighlightSelection.Sub:
                color = isOn ? subColor : neutralColor;
                tiles = subSelection;
                if (isOn) HighlightTiles(mainSelection, mainColor);
                break;
            default:
                break;
        }
        HighlightTiles(tiles, color);
        if (!isOn && sel == HighlightSelection.Sub)
        {
            HighlightTiles(mainSelection, mainColor);
        }
    }

    private void HighlightTiles(List<Tile> tiles, Color color)
    {
        if (tiles == null || tiles.Count <= 0) return;
        tiles.ForEach(x => x.Highlight(color));
    }
}
