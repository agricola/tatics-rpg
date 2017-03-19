using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path
{
    public LinkedList<GameObject> Tiles { get; private set; }
    public LinkedListNode<GameObject> Current { get; private set; }
    public Path(IEnumerable<GameObject> tiles, int limit = -1)
    {
        Tiles = new LinkedList<GameObject>(tiles);
        Tiles.RemoveFirst();
        while (limit > 0 && Tiles.Count > (limit))
        {
            Tiles.RemoveLast();
        }
        Current = Tiles.First;
    }

    public bool Advance()
    {
        bool nextTileExists = false;
        LinkedListNode<GameObject> next = Current.Next;
        if (next != null)
        {
            Current = next;
            nextTileExists = true;
        }
        return nextTileExists;
    }
}
