using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Explosive
{
    [SerializeField]
    private float timeToDetonate;
    private float detonateTime;

    private void OnEnable()
    {
        detonateTime = Time.time + timeToDetonate;
    }

    // Update is called once per frame
    void Update ()
    {
		if (Time.time >= detonateTime)
        {
            Explode();
        }
	}


}
