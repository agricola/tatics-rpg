using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Map map;
    [SerializeField]
    private List<Character> characters = new List<Character>();

    static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    public Map Map
    {
        get
        {
            return map;
        }
    }
    public List<Character> Characters
    {
        get
        {
            return characters;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        EventManager.Instance.AddListener<MapChangeEvent>(OnMapChange);
        EventManager.Instance.AddListener<CharacterChangeEvent>(OnCharacterChange);
    }

    private void OnDestroy()
    {
        EventManager em = EventManager.Instance;
        if (em)
        {
            EventManager.Instance.RemoveListener<MapChangeEvent>(OnMapChange);
            EventManager.Instance.RemoveListener<CharacterChangeEvent>(OnCharacterChange);
        }
    }

    private void OnMapChange(MapChangeEvent e)
    {
        map = e.map;
    }

    private void OnCharacterChange(CharacterChangeEvent e)
    {
        if (e.create)
        {
            characters.Add(e.character);
        }
        else
        {
            characters.Remove(e.character);
        }
    }
}
