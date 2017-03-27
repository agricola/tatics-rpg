using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    private bool acted = false;
    [SerializeField]
    private bool moved = false;
    [SerializeField]
    private bool isGood = true;
    [SerializeField]
    private bool isHighlighted = false;
    [SerializeField]
    private GameObject highlightGameObject;
    [SerializeField]
    private int movementLimit = 5;
    [SerializeField]
    private MapPosition mapPosition;

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
    public int MovementLimit
    {
        get
        {
            return movementLimit;
        }
    }
    public bool IsGood
    {
        get
        {
            return isGood;
        }
        set
        {
            if (!value) GetComponent<SpriteRenderer>().color = Color.red;
            isGood = value;
        }
    }
    public bool Acted
    {
        get
        {
            return acted;
        }
        set
        {
            acted = value;
        }
    }
    public bool Moved
    {
        get
        {
            return moved;
        }
        set
        {
            moved = value;
        }
    }

    private void Awake()
    {
        if (!highlightGameObject) highlightGameObject = transform.GetChild(0).gameObject;
        highlightGameObject.SetActive(false);
        EventManager.Instance.AddListener<CharacterSelectEvent>(OnCharacterSelect);
    }

    private void Start()
    {
        EventManager.Instance.Raise(new CharacterChangeEvent(this, true));
    }

    private void OnDeath()
    {
        EventManager.Instance.Raise(new CharacterChangeEvent(this, false));
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener<CharacterSelectEvent>(OnCharacterSelect);
    }

    private void OnMouseDown()
    { 
        EventManager.Instance.Raise(new CharacterSelectEvent(isGood, this));
    }

    private void OnCharacterSelect(CharacterSelectEvent e)
    {
        //if (e.character != this) ToggleHighlight(false);
    }

    public void ToggleHighlight()
    {
        isHighlighted = !isHighlighted;
        highlightGameObject.SetActive(isHighlighted);
    }

    public void ToggleHighlight(bool isHighlighted)
    {
        this.isHighlighted = isHighlighted;
        highlightGameObject.SetActive(isHighlighted);
    }

    public void ResetActions()
    {
        moved = false;
        acted = false;
    }
}

