using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    private Map map;

    public Dictionary<Tile, Tile> TravelDic(float x, float y, float goalX, float goalY)
    {
        return TravelDic((int)x, (int)y, (int)goalX, (int)goalY);
    }

    public Dictionary<Tile, Tile> TravelDic(int x, int y, int goalX, int goalY)
    {
        PriorityQueue frontier = new PriorityQueue();
        Tile start = map.Tiles[x, y];
        frontier.Enqueue(start, 0);
        Tile goal = map.Tiles[goalX, goalY];

        // <current, came_from>
        Dictionary<Tile, Tile> cameFrom = new Dictionary<Tile, Tile>();
        cameFrom[start] = null;
        Dictionary<Tile, int> costSoFar = new Dictionary<Tile, int>();
        costSoFar[start] = 0;

        while (frontier.Count > 0)
        {
            Tile current = frontier.Dequeue();
            if (current == goal)
            {
                break;
            }
            List<Tile> neighbors = map.GetNeighbors(
                (int)current.transform.localPosition.x,
                (int)current.transform.localPosition.y);
            foreach (var next in neighbors)
            {
                int cost = costSoFar[current] + next.GetComponent<Tile>().MovementCost;
                if (!costSoFar.ContainsKey(next) || cost < costSoFar[next])
                {
                    costSoFar[next] = cost;
                    int priority = cost + Heuristic(goal, next);
                    frontier.Enqueue(next, priority);
                    cameFrom[next] = current;
                    if (goal == next)
                    {
                        break;
                    }
                }
            }
        }
        return cameFrom;
    }

    public Path GetPath(Map map, float x, float y, float goalX, float goalY, int limit)
    {
        return GetPath(map, (int)x, (int)y, (int)goalX, (int)goalY, limit);
    }

    public Path GetPath(Map map, int x, int y, int goalX, int goalY, int limit)
    {
        this.map = map;
        Tile start = map.Tiles[x, y];
        Tile goal = map.Tiles[goalX, goalY];
        Dictionary<Tile, Tile> travelDic = TravelDic(x, y, goalX, goalY);
        LinkedList<Tile> path = new LinkedList<Tile>();
        Tile current = goal;
        path.AddLast(current);
        while (current != start)
        {
            try
            {
                current = travelDic[current];
                path.AddFirst(current);
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }
        Path finalPath = new Path(path, limit);
        return finalPath;
    }

    private int Heuristic(Tile current, Tile goal)
    {
        Vector2 c = current.transform.position;
        Vector2 g = goal.transform.position;
        return Math.Abs((int)c.x - (int)g.x)
            + Math.Abs((int)c.y - (int)g.y);
    }
}
