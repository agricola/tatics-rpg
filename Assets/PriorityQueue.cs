using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue
{

    private List<Tuple<GameObject, double>> elements = new List<Tuple<GameObject, double>>();

    public int Count
    {
        get
        {
            return elements.Count;
        }
    }
    public List<Tuple<GameObject, double>> Elements
    {
        get
        {
            return elements;
        }
    }

    public void Enqueue(GameObject item, double priority)
    {
        elements.Add(Tuple.Create(item, priority));
    }

    public GameObject Dequeue()
    {
        int bestIndex = 0;
        for (int i = 0; i < elements.Count; i++)
        {
            if (elements[i].Item2 < elements[bestIndex].Item2)
            {
                bestIndex = i;
            }
        }
        GameObject bestItem = elements[bestIndex].Item1;
        elements.RemoveAt(bestIndex);
        return bestItem;
    }
}
