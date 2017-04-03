using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BattleGroup
{
    [SerializeField]
    private List<Character> members;
    [SerializeField]
    private bool isGood;

    public List<Character> Members
    {
        get
        {
            UpdateMembers();
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

    private void UpdateMembers()
    {
        members.RemoveAll(x => x == null);
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

