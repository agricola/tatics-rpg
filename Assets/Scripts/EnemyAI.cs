using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private Map map;
    readonly int limit = 99;

    public IEnemyStrategy EnemyStrategy { get; private set; }
    public Path WalkPath { get; private set; }

    public void DetermineStrategy(List<Character> targets, Map map)
    {
        Character self = GetComponent<Character>();
        CommandManager commandManager = CommandManager.Instance;
        if (!self)
        {
            Debug.Log("Cannot find Character component for enemy");
            return;
        }
        Queue<Character> closestTargets = ClosestTargets(targets, self);
        Func<Map, MapPosition, Path> findPath = GetPath(self.MapPosition, limit);
        Path path = FindBestPath(map, closestTargets, findPath);
        WalkPath = path;
    }

    private Func<Map, MapPosition, Path> GetPath(MapPosition source, int limit)
    {
        Pathfinder pathfinder = Pathfinder.Instance;
        Func<Map, MapPosition, Path> findPath =
            (Map map, MapPosition mapPos) =>
            pathfinder.GetPath(map, source.X, source.Y, mapPos.X, mapPos.Y, limit);
        return findPath;
    }
    /*
    private Tile ClosestTile(MapPosition target, Map map, MapPosition self)
    {
        List<Tile> tiles = map.GetNeighbors(target);
        if (tiles.Count <= 0) return null;
        Tile closest = tiles[0];
        int lowestDistance = DistanceBetween(self, closest.MapPosition;
        tiles.RemoveAt(0);
        if (tiles.Count > 0)
        {
            foreach (var tile in tiles)
            {
                int newDistance = DistanceBetween(self, tile.MapPosition);
                if (newDistance < lowestDistance)
                {
                    lowestDistance = newDistance;
                    closest = tile;
                }
            }
        }
        return closest;
    }*/

    private Path FindBestPath(Map map, Queue<Character> closestTargets, Func<Map, MapPosition, Path> findPath)
    {
        Queue<Character> targets = closestTargets;
        Path bestPath = null;
        while (bestPath == null)
        {
            Character target = targets.Dequeue();
            map.TileAtMapPosition(target.MapPosition).Occupant = null;
            bestPath = findPath(map, target.MapPosition);
            map.TileAtMapPosition(target.MapPosition).Occupant = target.gameObject;
            if (targets.Count <= 0)
            {
                break;
            }
        }
        return bestPath;
    }

    private Queue<Character> ClosestTargets(List<Character> targets, Character self)
    {
        MapPosition selfPos = self.MapPosition;
        SortedDictionary<int, List<Character>> characterByDistance = new SortedDictionary<int, List<Character>>();
        foreach (Character target in targets)
        {
            int distance = DistanceBetween(selfPos, target.MapPosition);
            if (characterByDistance.ContainsKey(distance))
            {
                characterByDistance[distance].Add(target);
            }
            else
            {
                characterByDistance[distance] = new List<Character>() { target };
            }
        }
        return EnQueueCharacters(characterByDistance);
    }

    // helper to ClosestTargets to make a Queue from the dictionary
    private Queue<Character> EnQueueCharacters(SortedDictionary<int, List<Character>> characterByDistance)
    {
        Queue<Character> characterQueue = new Queue<Character>();
        foreach (var characterList in characterByDistance)
        {
            foreach (var character in characterList.Value)
            {
                characterQueue.Enqueue(character);
            }
        }
        return characterQueue;
    }

    private int DistanceBetween(MapPosition self, MapPosition target)
    {
        return Math.Abs(self.X - target.X)
        + Math.Abs(self.Y - target.Y);
    }
}