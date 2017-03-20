using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path
{
    public LinkedList<Tile> Tiles { get; private set; }
    public LinkedListNode<Tile> Current { get; private set; }
    public Path(IEnumerable<Tile> tiles, int limit = -1)
    {
        Tiles = new LinkedList<Tile>(tiles);
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
        LinkedListNode<Tile> next = Current.Next;
        if (next != null)
        {
            Current = next;
            nextTileExists = true;
        }
        return nextTileExists;
    }
}
