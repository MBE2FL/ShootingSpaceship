using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    [SerializeField]
    private float force;
    [SerializeField]
    private float radius;
    [SerializeField]
    private GameObject effect;

	//// Use this for initialization
	//void Start ()
 //   {
		
	//}
	
	//// Update is called once per frame
	//void Update ()
 //   {
		
	//}

    public void Explode()
    {
        // Find all colliders within this explosion's radius.
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider collider in colliders)
        {
            // Verify each game object has a rigidbody.
            if (!collider.attachedRigidbody)
                continue;

            // Apply the force to the game object's rigidbody.
            collider.attachedRigidbody.AddExplosionForce(force, transform.position, radius, 0.0f, ForceMode.Impulse);
        }

        Instantiate(effect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
