using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectEvent : GameEvent
{
    public bool isGood;
    public Character character;
    public CharacterSelectEvent(bool isGood, Character character)
    {
        this.isGood = isGood;
        this.character = character;
    }
}

public class Character : MonoBehaviour
{
    [SerializeField]
    private bool isGood = true;
    [SerializeField]
    private bool isHighlighted = false;
    [SerializeField]
    private GameObject highlightGameObject;
    [SerializeField]
    private int movementLimit = 5;

    public int MovementLimit
    {
        get
        {
            return movementLimit;
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

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener<CharacterSelectEvent>(OnCharacterSelect);
        EventManager.Instance.Raise(new CharacterChangeEvent(this, false));
    }

    private void OnMouseDown()
    {
        EventManager.Instance.Raise(new CharacterSelectEvent(isGood, this));
    }

    private void OnCharacterSelect(CharacterSelectEvent e)
    {
        if (e.character != this) ToggleHighlight(false);
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
}

