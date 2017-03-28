﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    private GameObject map;
    private Pathfinder pathfinder;

    private Path path;
    private List<Tile> oldPath = new List<Tile>();
    private int limit;
    private bool findingPath = false;
    private void OnDestroy()
    {
        //ResetPath();
        EventManager.Instance.RemoveListener<PathfindEvent>(OnPathfindEvent);
        EventManager.Instance.RemoveListener<MoveCharacterEvent>(OnMoveEvent);
        EventManager.Instance.RemoveListener<FightEvent>(OnFight);
    }

    private void Start()
	{
        pathfinder = GameObject.Find("PathfindingManager").GetComponent<Pathfinder>();
        EventManager.Instance.AddListener<PathfindEvent>(OnPathfindEvent);
        EventManager.Instance.AddListener<MoveCharacterEvent>(OnMoveEvent);
        EventManager.Instance.AddListener<FightEvent>(OnFight);
    }
	
	private void Update()
	{
		if (!map) map = GameManager.Map.gameObject;
    }

    private void OnPathfindEvent(PathfindEvent e)
    {
        if (e is PathfindCreateEvent)
        {
            if (findingPath) return;
            findingPath = true;
            PathfindCreateEvent ev = e as PathfindCreateEvent;
            limit = ev.limit;
            FindPath(ev.source, ev.goal);
            findingPath = false;
        }
        else if (e is CancelPathfindEvent)
        {
            CancelPathfind();
        }
    }

    private void OnFight(FightEvent e)
    {
        Debug.Log(e.Attacker + " fights " + e.Defender);
    }

    private void CancelPathfind()
    {
        ResetPath();
    }

    private void FindPath(Tile source, Tile goal)
    {
        MapPosition sPos = source.MapPosition;
        MapPosition gPos = goal.MapPosition;

        ResetOldPath();
        path = pathfinder.GetPath(GameManager.Map, sPos.X, sPos.Y, gPos.X, gPos.Y, limit);
        HighlightTiles();
    }
    
    private void ResetOldPath()
    {
        if (oldPath.Count <= 0) return;
        foreach (var tile in oldPath)
        {
            if (tile) tile.Highlight(HighlightType.Old);
        }
        oldPath = new List<Tile>();
        path = null;
    }

    private void ResetPath()
    {
        oldPath = new List<Tile>();
        path = null;
        EventManager.Instance.Raise(new UnhighlightTilesEvent());
    }

    private void HighlightTiles()
    {
        if (path != null)
        {
            if (path.Current == null) return;
            path.Current.Value.Highlight(HighlightType.Targeting);
            oldPath.Add(path.Current.Value.GetComponent<Tile>());
            while (path.Advance())
            {
                path.Current.Value.GetComponent<Tile>().Highlight(HighlightType.Targeting);
                oldPath.Add(path.Current.Value.GetComponent<Tile>());
            }
            //Debug.Log("exit");
        }
    }

    private void OnMoveEvent(MoveCharacterEvent e)
    {
        LinkedList<Tile> movement = path.Tiles;
        ResetPath();
        EventManager.Instance.Raise(new InputToggleEvent(false));
        EventManager.Instance.Raise<AnimationEvent>(new ToggleWalkEvent(true, e.character.gameObject));
        StartCoroutine(MoveCharacter(movement, e.character));
    }

    private IEnumerator MoveCharacter(LinkedList<Tile> tiles, Character c)
    {
        foreach (var tile in tiles)
        {
            yield return new WaitForSeconds(0.2f);
            if (!tile.MoveObjectTo(c.gameObject))
            {
                break;
            }
        }
        EventManager.Instance.Raise(new InputToggleEvent(true));
        EventManager.Instance.Raise<AnimationEvent>(new ToggleWalkEvent(false, c.gameObject));
    }
}
