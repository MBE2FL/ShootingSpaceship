using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastGun : MonoBehaviour {

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

            // Draw a line and display the name of anything the ray hit
            // Deal damage to any enemies which were hit
            if (Physics.Raycast(ray, out hit))
            {
                Debug.DrawRay(ray.origin, ray.direction * 50.0f, Color.blue, 1.5f);
                Debug.Log(hit.collider);

                if (hit.collider.tag == "Enemy")
                    hit.collider.gameObject.GetComponent<EnemyAI>().loseHealth(5);
            }
        }

	}
}
