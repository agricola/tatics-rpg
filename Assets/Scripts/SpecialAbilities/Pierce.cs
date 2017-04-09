using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pierce : ISpecial
{
    [SerializeField]
    private readonly SelectType selectType = SelectType.Line;
    [SerializeField]
    private readonly TargetType targetType = TargetType.Tile;

    public SelectType SelectType
    {
        get
        {
            return selectType;
        }
    }
    public TargetType TargetType
    {
        get
        {
            return targetType;
        }
    }

    public void Perform(Character actor, GameObject target = null)
    {
        // Start Attack animation
        // At hit event, turn actor into line
        // Move actor through line
        // Turn actor back into orginal sprite
        // Deal damage to everything passed through
    }
}
