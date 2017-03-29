using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour {

    static AIManager instance;
    public static AIManager Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void DetermineStrategies(List<Character> good, List<Character> bad, Map map)
    {
        foreach (var enemy in bad)
        {
            enemy.GetComponent<EnemyAI>().DetermineStrategy(good,map);
        }
    }
}
