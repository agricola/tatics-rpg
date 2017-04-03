using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private Map map;
    //private readonly int limit = 5; // temp, replace with movement limit in future

    public IEnemyStrategy EnemyStrategy { get; private set; }
    public Path WalkPath { get; private set; }

    public Path DetermineMoveStrategy(List<Character> targets, Map map)
    {
        Character self = GetComponent<Character>();
        CommandManager commandManager = CommandManager.Instance;
        if (!self)
        {
            Debug.Log("Cannot find Character component for enemy");
            return null;
        }
        Queue<Character> closestTargets = ClosestTargets(targets, self);
        Func<Map, MapPosition, Path> findPath = GetPath(self.MapPosition, self.MovementLimit);
        return FindBestPath(map, closestTargets, findPath);
    }

    // functional programming meme helper
    private Func<Map, MapPosition, Path> GetPath(MapPosition source, int limit)
    {
        Pathfinder pathfinder = Pathfinder.Instance;
        Func<Map, MapPosition, Path> findPath =
            (Map map, MapPosition mapPos) =>
            pathfinder.GetPath(map, source.X, source.Y, mapPos.X, mapPos.Y, limit);
        return findPath;
    }

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

    public Character DetermineActionStrategy(List<Character> targets, Map map)
    {
        MapPosition selfPos = GetComponent<Character>().MapPosition;
        List<Character> neighbors = new List<Character>();
        foreach (Character target in targets)
        {
            if (map.AreNeighbors(selfPos, target.MapPosition)) neighbors.Add(target);
        }
        return ChooseTarget(neighbors);
    }

    // choose target when they are all neighbors
    private Character ChooseTarget(List<Character> targets)
    {
        return targets.Count > 0 ? targets[0] : null;
    }
}