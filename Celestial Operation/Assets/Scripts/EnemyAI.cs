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
    public Transform hitText;


    private float startTime;
    private float currentTime;


    void Awake()
    {
        enemyManager = GameObject.Find("Managers").GetComponent<EnemyManager>();
    }

    void OnEnable()
    {
        health = enemyManager.EnemyHealth;
        startTime = Time.time;
        currentTime = 0.0f;
    }

    // Use this for initialization
    void Start ()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        //rb = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        //enemyManager = GameObject.Find("Managers").GetComponent<EnemyManager>();
        //health = enemyManager.EnemyHealth;
    }
	
	// Update is called once per frame
	void Update ()
    {

        if (navMeshAgent.enabled)
        {
            // Update Nav Mesh Agent with the player's position
            navMeshAgent.SetDestination(playerTransform.position);
        }


        // After waiting a set time, reload the gun
        if (currentTime >= 1.0f)
        {
            currentTime = 0.0f;
            startTime = Time.time;
            Shoot();
        }
        // Track time till this weapon is reloaded
        else if (currentTime < 1.0f)
        {
            currentTime = Time.time - startTime;
        }

    }

    //void FixedUpdate()
    //{
    //    // Get direction from this enemy to the player
    //    Vector3 direction = (playerTransform.position - transform.position).normalized;

    //    // Apply a force in the given direction
    //    rb.AddForce(direction * movementSpeed);
    //}

    public void loseHealth(int amount)
    {
        // Display amount of damage taken, as floating text
        FloatText floatText = ObjectPooler.Instance.SpawnObject("FloatText", transform.position, Camera.main.transform.rotation).GetComponent<FloatText>();
        floatText.Message = amount.ToString();

        // Lose health
        health -= amount;

        // Destroy this enemy after health has reached zero
        if (health <= 0)
        {
            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }


    void Shoot()
    {
        Vector3 bulletSpawn = transform.position + (transform.forward * 2);
        Vector3 bulletDirection = (enemyManager.player.position - transform.position).normalized;

        GameObject currentBullet = ObjectPooler.Instance.SpawnObject("Bullet", bulletSpawn, transform.rotation);
        BulletLogic bulletLogic = currentBullet.GetComponent<BulletLogic>();
        bulletLogic.Damage = 25;
        bulletLogic.applyForce(bulletDirection * 1000.0f);
        bulletLogic.OwnerTransform = gameObject.GetComponentInParent<Transform>();
    }
}
