using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    private Map map;

    public Dictionary<GameObject, GameObject> TravelDic(float x, float y, float goalX, float goalY)
    {
        return TravelDic((int)x, (int)y, (int)goalX, (int)goalY);
    }

    public Dictionary<GameObject, GameObject> TravelDic(int x, int y, int goalX, int goalY)
    {
        PriorityQueue frontier = new PriorityQueue();
        GameObject start = map.Tiles[x, y].gameObject;
        frontier.Enqueue(start, 0);
        GameObject goal = map.Tiles[goalX, goalY].gameObject;

        // <current, came_from>
        Dictionary<GameObject, GameObject> cameFrom = new Dictionary<GameObject, GameObject>();
        cameFrom[start] = null;
        Dictionary<GameObject, int> costSoFar = new Dictionary<GameObject, int>();
        costSoFar[start] = 0;

        while (frontier.Count > 0)
        {
            GameObject current = frontier.Dequeue();
            if (current == goal)
            {
                break;
            }
            List<GameObject> neighbors = map.GetNeighbors(
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

    public Path GetPath(Map map, float x, float y, float goalX, float goalY)
    {
        return GetPath(map, (int)x, (int)y, (int)goalX, (int)goalY);
    }

    public Path GetPath(Map map, int x, int y, int goalX, int goalY)
    {
        this.map = map;
        GameObject start = map.Tiles[x, y].gameObject;
        GameObject goal = map.Tiles[goalX, goalY].gameObject;
        Dictionary<GameObject, GameObject> travelDic = TravelDic(x, y, goalX, goalY);
        LinkedList<GameObject> path = new LinkedList<GameObject>();
        GameObject current = goal;
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
        Path finalPath = new Path(path);
        return finalPath;
    }

    private int Heuristic(GameObject current, GameObject goal)
    {
        Vector2 c = current.transform.position;
        Vector2 g = goal.transform.position;
        return Math.Abs((int)c.x - (int)g.x)
            + Math.Abs((int)c.y - (int)g.y);
    }
}
