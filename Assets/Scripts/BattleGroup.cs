using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGroup
{
    private List<Character> members;
    private bool isGood;

    public List<Character> Members
    {
        get
        {
            return new List<Character>(members);
        }
    }
    public bool IsGood
    {
        get
        {
            return isGood;
        }
    }

    public BattleGroup(List<Character> members, bool isGood)
    {
        this.members = members;
        this.isGood = isGood;
    }

    public void ResetAllActions()
    {
        foreach (var member in members)
        {
            member.ResetActions();
        }
    }
}

