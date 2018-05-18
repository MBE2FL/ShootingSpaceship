using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour {


    private Transform playerTransform;
    //private Rigidbody rb;
    private NavMeshAgent navMeshAgent;
    private EnemyManager enemyManager;
    private int health;


    // Use this for initialization
    void Start ()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        //rb = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyManager = GameObject.Find("Managers").GetComponent<EnemyManager>();
        health = enemyManager.EnemyHealth;
    }
	
	// Update is called once per frame
	void Update ()
    {

        // Update Nav Mesh Agent with the player's position
        navMeshAgent.SetDestination(playerTransform.position);

    }

    void FixedUpdate()
    {
        //// Get direction from this enemy to the player
        //Vector3 direction = (playerTransform.position - transform.position).normalized;

        //// Apply a force in the given direction
        //rb.AddForce(direction * movementSpeed);
    }

    public void loseHealth(int amount)
    {
        // Lose health
        health -= amount;

        // Destroy this enemy after health has reached zero
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
