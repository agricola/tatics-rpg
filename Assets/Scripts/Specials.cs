using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class Specials : MonoBehaviour
{
    private List<ISpecial> knownSpecials;
    public ReadOnlyCollection<ISpecial> KnownSpecials
    {
	get
	{
	    return knownSpecials.AsReadOnly();
	}
    }

    void Start()
    {
    }
}
