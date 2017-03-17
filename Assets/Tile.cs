using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private Color neutralColor = Color.white;
    private Color highlightedColor = Color.red;

    private void Awake()
    {
		//neutralColor = GetComponent<Renderer>().material.color;
    }
	
	private void Update()
    {
		
	}

    private void OnMouseEnter()
    {
        GetComponent<Renderer>().material.color = highlightedColor;
    }
    private void OnMouseExit()
    {
        GetComponent<Renderer>().material.color = neutralColor;
    }

}
