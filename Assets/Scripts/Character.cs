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

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener<DeselectEvent>(OnDeselectEvent);
    }

    private void OnMouseDown()
    {
        CharacterSelectEvent e = new CharacterSelectEvent(isGood, this);
        EventManager.Instance.Raise<CharacterSelectEvent>(e);
        ToggleHighlight();
    }

    private void OnDeselectEvent(DeselectEvent e)
    {
        ToggleHighlight();
    }

    private void ToggleHighlight()
    {
        isHighlighted = !isHighlighted;
        highlightGameObject.SetActive(isHighlighted);
    }
}

