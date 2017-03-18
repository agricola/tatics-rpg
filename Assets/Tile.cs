using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileEvent : GameEvent
{
    public bool highlighted;
    public Tile tile;
    public TileEvent(Tile tile, bool highlighted)
    {
        this.tile = tile;
        this.highlighted = highlighted;
    }
}

public class Tile : MonoBehaviour
{
    [SerializeField]
    private bool blocked = false;
    [SerializeField]
    private Color neutralColor = Color.white;
    [SerializeField]
    private Color highlightedColor = Color.red;
    [SerializeField]
    private int movementCost = 1;
    [SerializeField]
    private MapPosition mapPosition;

    public bool Blocked
    {
        get
        {
            return blocked;
        }
    }
    public int MovementCost
    {
        get
        {
            return movementCost;
        }
    }
    public MapPosition MapPosition
    {
        get
        {
            return mapPosition;
        }
        set
        {
            mapPosition = value;
        }
    }
    /*
    public Tile(int x, int y)
    {
        mapPosition = new MapPosition(x, y);
    }*/

    public void Highlight(bool isHighlighted)
    {
        GetComponent<Renderer>().material.color
            = isHighlighted ? highlightedColor : neutralColor;
    }
    private void OnMouseDown()
    {
        TileEvent e = new TileEvent(this, false);
        EventManager.Instance.Raise<TileEvent>(e);
    }
	
	private void Update()
    {
		
	}

    private void OnMouseEnter()
    {
    }
    private void OnMouseExit()
    {
    }

}
