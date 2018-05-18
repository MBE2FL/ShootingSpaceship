using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : MonoBehaviour {

    
    public Rigidbody rigidbody;
    public float lifeTime = 2.0f;

    private int damage;
    private float startTime;
    private float currentTime;
    

    public int Damage
    {
        get
        {
            return damage;
        }

        set
        {
            damage = value;
        }
    }

    // Use this for initialization
    void Start ()
    {
        //rigidbody = GetComponent<Rigidbody>();
        startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Destroy this bullet after some time has passed
        currentTime = Time.time - startTime;

        if (currentTime >= lifeTime)
        {
            Destroy(gameObject);
        }
	}

    public void applyForce(Vector3 force)
    {
        rigidbody.AddForce(force);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Deal damage to an enemy
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyAI>().loseHealth(damage);
            
            // Destroy this bullet
            Destroy(gameObject);
        }
    }
}
