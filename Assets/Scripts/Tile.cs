using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileSelectType { None, Highlight, Move }

public class TileSelectEvent : GameEvent
{
    public TileSelectType selectType;
    public Tile tile;
    public TileSelectEvent(Tile tile, TileSelectType selectType)
    {
        this.tile = tile;
        this.selectType = selectType;
    }
}

public class Tile : MonoBehaviour
{
    [SerializeField]
    private bool blocked = false;
    [SerializeField]
    private Color neutralColor = Color.white;
    [SerializeField]
    private Color highlightedColor = Color.yellow;
    [SerializeField]
    private int movementCost = 1;
    [SerializeField]
    private MapPosition mapPosition;
    [SerializeField]
    private GameObject occupant;

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
    public GameObject Occupant
    {
        get
        {
            return occupant;
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

    private void OnMouseEnter()
    {
        RaiseTileEvent(TileSelectType.Highlight);
    }
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaiseTileEvent(TileSelectType.Move);
        }
    }

    private void RaiseTileEvent(TileSelectType selectType)
    {
        TileSelectEvent e = new TileSelectEvent(this, selectType);
        EventManager.Instance.Raise(e);
    }

    public void MoveObjectTo(GameObject obj)
    {
        Vector3 pos = transform.position;
        pos.z = -0.01f;
        obj.transform.position = pos;
        occupant = obj;
        EventManager.Instance.Raise(new TileChangeEvent());
        EventManager.Instance.AddListener<TileChangeEvent>(OnTileChange);
    }

    private void OnTileChange(TileChangeEvent e)
    {
        occupant = null;
        EventManager.Instance.RemoveListener<TileChangeEvent>(OnTileChange);
    }

}
