using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
    [SerializeField]
    private int damage = 5;

    private Collider2D coll;

    public int Damage
    {
        get
        {
            return damage;
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
        
        coll = GetComponent<Collider2D>();
    }

    private void Start()
    {
        // EventManager.Instance.Raise(new CharacterChangeEvent(this, true));
        
    }
    private void OnEnable()
    {
        EventManager.Instance.AddListener<ColliderToggleEvent>(OnColliderToggle);
    }

    private void OnDisable()
    {
        EventManager em = EventManager.Instance;
        if (em)
        {
            em.RemoveListener<ColliderToggleEvent>(OnColliderToggle);
        }
    }

    private void OnMouseDown()
    {

        if (!Acted && !EventSystem.current.IsPointerOverGameObject())
        {
            EventManager.Instance.Raise(new CharacterSelectEvent(isGood, this));
        }
    }

    private void OnColliderToggle(ColliderToggleEvent e)
    {
        coll.enabled = e.Enabled;
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

