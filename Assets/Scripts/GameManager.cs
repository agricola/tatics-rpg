using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Map currentMap;
    [SerializeField]
    private MapTiles currentMapTiles;
    [SerializeField]
    private List<MapTiles> levelMaps;

    public List<MapTiles> LevelMaps
    {
        get
        {
            return new List<MapTiles>(levelMaps);
        }
    }

    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    public Map CurrentMap
    {
        get
        {
            return currentMap;
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
        
    }

    private void OnEnable()
    {
        EventManager em = EventManager.Instance;
        if (em)
        {
            em.AddListener<MapChangeEvent>(OnMapChange);
            em.AddListener<FinishGeneratingMapsEvent>(OnFinishGeneratingMaps);
            em.AddListener<SelectLevelEvent>(OnSelectLevel);
        }
        
    }

    private void OnDisable()
    {
        EventManager em = EventManager.Instance;
        if (em)
        {
            em.RemoveListener<MapChangeEvent>(OnMapChange);
            em.RemoveListener<FinishGeneratingMapsEvent>(OnFinishGeneratingMaps);
            em.RemoveListener<SelectLevelEvent>(OnSelectLevel);
        }
    }

    private void OnFinishGeneratingMaps(FinishGeneratingMapsEvent e)
    {
        levelMaps = e.Maps;
    }

    private void OnMapChange(MapChangeEvent e)
    {
        currentMap = e.map;
        currentMap.BuildMap(currentMapTiles);
    }

    private void OnSelectLevel(SelectLevelEvent e)
    {
        SetMapTiles(e.SelectedLevel);
    }

    private void SetMapTiles(int mapIndex)
    {
        currentMapTiles = levelMaps[mapIndex];
    }
}
