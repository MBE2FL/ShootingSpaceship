using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour {

    private RaycastGun raycastGun;
    private PrefabGun prefabGun;

	// Use this for initialization
	void Start ()
    {
        raycastGun = gameObject.GetComponent<RaycastGun>();
        prefabGun = gameObject.GetComponent<PrefabGun>();

        raycastGun.enabled = true;
        prefabGun.enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (Input.GetKeyDown(KeyCode.Q))
        {
            raycastGun.enabled = !raycastGun.isActiveAndEnabled;
            prefabGun.enabled = !prefabGun.isActiveAndEnabled;
        }
	}
}
