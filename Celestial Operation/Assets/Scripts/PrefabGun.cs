using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabGun : MonoBehaviour {

    public Rigidbody bullet;

    private Camera cam;

    // Use this for initialization
    void Start ()
    {
        cam = Camera.main;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Mouse position on the screen (In pixels)
            Vector3 mousePos = Input.mousePosition;
            //Debug.Log("Local: " + mousePos);

            // Cast a ray from the mouse position though the camera, into the world
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(mousePos);

            // Shoot a bullet
            Rigidbody currBullet = Instantiate(bullet, ray.origin + cam.transform.forward, cam.transform.rotation);
            currBullet.GetComponent<BulletLogic>().Damage = 5;
            currBullet.GetComponent<BulletLogic>().applyForce(ray.direction * 1000.0f);
            
        }
    }
}
