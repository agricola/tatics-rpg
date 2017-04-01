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

    public void AddMember(Character character)
    {
        members.Add(character);
    }

    public void RemoveMember(Character character)
    {
        if (members.Contains(character)) members.Remove(character);
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

