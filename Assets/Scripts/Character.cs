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

    private void Awake()
    {
        if (!highlightGameObject) highlightGameObject = transform.GetChild(0).gameObject;
        highlightGameObject.SetActive(false);
        EventManager.Instance.AddListener<DeselectEvent>(OnDeselectEvent);
    }

    private void Start()
    {
        EventManager.Instance.Raise(new CharacterChangeEvent(this, true));
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener<DeselectEvent>(OnDeselectEvent);
        EventManager.Instance.Raise(new CharacterChangeEvent(this, false));
    }

    private void OnMouseDown()
    {
        EventManager.Instance.Raise(new CharacterSelectEvent(isGood, this));
    }

    private void OnDeselectEvent(DeselectEvent e)
    {
        ToggleHighlight();
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

