using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Map map;

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

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        EventManager.Instance.AddListener<MapChangeEvent>(OnMapChange);
    }

    private void OnDisable()
    {
        EventManager em = EventManager.Instance;
        if (em)
        {
            EventManager.Instance.RemoveListener<MapChangeEvent>(OnMapChange);
        }
    }

    private void OnMapChange(MapChangeEvent e)
    {
        map = e.map;
    }
}
