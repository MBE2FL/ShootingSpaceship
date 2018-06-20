using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : MonoBehaviour {

    
    public Rigidbody rigidbody;
    public float lifeTime = 2.0f;

    private int damage;
    private float startTime;
    private float currentTime;
    private Transform ownerTransform;

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

    public Transform OwnerTransform
    {
        get
        {
            return ownerTransform;
        }

        set
        {
            ownerTransform = value;
        }
    }

    // Use this for initialization
    void OnEnable ()
    {
        // Reset bullet, after being recyled through it's memory pool
        rigidbody.velocity = Vector3.zero;
        startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Destroy this bullet after some time has passed
        currentTime = Time.time - startTime;

        if (currentTime >= lifeTime)
        {
            //Destroy(gameObject);
            gameObject.SetActive(false);
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
            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
        // Collided with a player shield
        else if (collision.collider.name == "Field")
        {
            // Determine which mode the player's shield is in
            Shield shield = collision.gameObject.GetComponentInParent<Shield>();

            switch (shield.CurrentMode)
            {
                case Shield.Mode.Absorb:
                    {
                        shield.Ability(damage);
                        gameObject.SetActive(false);
                        break;
                    }
                case Shield.Mode.Deflect:
                    {
                        shield.Ability(rigidbody, ownerTransform.position, damage);
                        break;
                    }
                default:
                    {
                        Debug.LogError("Shield Abnormality");
                        break;
                    }
            }
        }
    }
}
