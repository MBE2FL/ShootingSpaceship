using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    private int health;
    [SerializeField]
    private string name;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    /// <summary>
    /// Used to decrease the health of this character.
    /// </summary>
    /// <param name="amount">The amount of health to lose.</param>
    public void loseHealth(int amount)
    {
        health -= amount;
    }
}
