using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

public class Path
{
    private LinkedList<Tile> tiles;
    private LinkedListNode<Tile> current;
    public ReadOnlyCollection<Tile> Tiles
    {
        get
        {
            return tiles.ToList().AsReadOnly();
        }
    }
    public Tile Current
    {
        get
        {
            return current.Value;
        }
    }
    public Path(IEnumerable<Tile> t, int limit = -1)
    {
        tiles = new LinkedList<Tile>(t);
        tiles.RemoveFirst();
        while (limit > 0 && tiles.Count > (limit))
        {
            tiles.RemoveLast();
        }
        current = tiles.First;
    }

    public bool Advance()
    {
        bool nextTileExists = false;
        if (current.Next != null)
        {
            current = current.Next;
            nextTileExists = true;
        }
        return nextTileExists;
    }
}
