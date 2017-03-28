using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum HighlightType { None, Old, Targeting, Radius }

public class Tile : MonoBehaviour
{
    [SerializeField]
    private bool blocked = false;
    [SerializeField]
    private Color neutralColor = Color.white;
    [SerializeField]
    private Color targetingColor = Color.yellow;
    [SerializeField]
    private Color radiusColor = Color.red;
    [SerializeField]
    private Color oldColor = Color.white;
    [SerializeField]
    private int movementCost = 1;
    [SerializeField]
    private MapPosition mapPosition;
    [SerializeField]
    private GameObject occupant;
    private bool modified = false;

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

    public void Highlight(HighlightType highlight)
    {
        Color color = neutralColor;
        switch (highlight)
        {
            case HighlightType.None:
                color = neutralColor;
                break;
            case HighlightType.Old:
                color = oldColor;
                break;
            case HighlightType.Targeting:
                color = targetingColor;
                break;
            case HighlightType.Radius:
                color = radiusColor;
                break;
            default:
                break;
        }
        if (!GetComponent<Renderer>()) return;
        Color possibleOldColor = GetComponent<Renderer>().material.color;
        if (possibleOldColor != targetingColor) oldColor = possibleOldColor;
        if (color != neutralColor)
        {
            EventManager.Instance.AddListener<UnhighlightTilesEvent>(OnUnhighlightTilesEvent);
            modified = true;
        }
        else if (modified)
        {
            EventManager.Instance.RemoveListener<UnhighlightTilesEvent>(OnUnhighlightTilesEvent);
            modified = false;
        }
        GetComponent<Renderer>().material.color = color;
    }

    private void OnUnhighlightTilesEvent(UnhighlightTilesEvent e)
    {
        //EventManager.Instance.RemoveListener<UnhighlightTilesEvent>(OnUnhighlightTilesEvent);
        Highlight(HighlightType.None);
    }

    private void OnMouseEnter()
    {
        RaiseTileEvent(TileSelectType.Highlight);
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        RaiseTileEvent(TileSelectType.Cancel);
    }

    private void OnMouseExit()
    {
        //EventManager.Instance.Raise<PathfindEvent>(new CancelPathfindEvent());
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaiseTileEvent(TileSelectType.Move);
        }
    }

    private void OnDestroy()
    {
        if (modified) EventManager.Instance.RemoveListener<UnhighlightTilesEvent>(OnUnhighlightTilesEvent);
    }

    private void RaiseTileEvent(TileSelectType selectType)
    {
        TileSelectEvent e = new TileSelectEvent(this, selectType);
        EventManager.Instance.Raise(e);
    }

    public bool MoveObjectTo(GameObject obj)
    {
        if (!isWalkable()) return false;
        Vector3 pos = transform.position;
        pos.z = obj.transform.position.z;
        AnimationManager anim = obj.GetComponent<AnimationManager>();
        if (anim)
        {
            anim.CheckScale(pos.x - obj.transform.position.x);
        }
        obj.transform.position = pos;
        occupant = obj;
        Character c = occupant.GetComponent<Character>();
        if (c) c.MapPosition = mapPosition;
        EventManager.Instance.Raise(new TileChangeEvent(obj));
        EventManager.Instance.AddListener<TileChangeEvent>(OnTileChange);
        return true;
    }

    public bool isWalkable()
    {
        return !blocked && occupant == null;
    }

    private void OnTileChange(TileChangeEvent e)
    {
        if (e.leaver == occupant)
        {
            occupant = null;
            EventManager.Instance.RemoveListener<TileChangeEvent>(OnTileChange);
        }
    }

}
