using System.Collections.Generic;

public class PriorityQueue
{

    private List<Tuple<Tile, double>> elements = new List<Tuple<Tile, double>>();

    public int Count
    {
        get
        {
            return elements.Count;
        }
    }
    public List<Tuple<Tile, double>> Elements
    {
        get
        {
            return elements;
        }
    }

    public void Enqueue(Tile item, double priority)
    {
        elements.Add(Tuple.Create(item, priority));
    }

    public Tile Dequeue()
    {
        int bestIndex = 0;
        for (int i = 0; i < elements.Count; i++)
        {
            if (elements[i].Item2 < elements[bestIndex].Item2)
            {
                bestIndex = i;
            }
        }
        Tile bestItem = elements[bestIndex].Item1;
        elements.RemoveAt(bestIndex);
        return bestItem;
    }
}
