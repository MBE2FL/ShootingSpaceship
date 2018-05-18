using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    public int maxEnemies = 5;
    public float spawnDelay = 1.0f;
    public Transform testEnemy;
    public int enemyHealth = 10;

    private static EnemyManager instance;
    private int currentEnemies = 0;
    private float currentTime = 0.0f;
    private float startTime;


    public static EnemyManager Instance
    {
        get
        {
            return instance;
        }
    }

    public int EnemyHealth
    {
        get
        {
            return enemyHealth;
        }
    }

    private void Awake()
    {
        // Singleton object
        if (!instance)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start ()
    {
        startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Track how much time has passed between enemy spawns
        currentTime = Time.time - startTime;

        // After enough time has passed, spawn a new enemy
        if ((currentTime >= spawnDelay) && (currentEnemies < maxEnemies))
        {
            // Generate a random spawn position
            Vector3 spawnPos = new Vector3(Random.Range(-10.0f, 10.0f), 0.5f, 9.0f);

            // Reset startTime and spawn a new enemy
            startTime = Time.time;
            Instantiate(testEnemy, spawnPos, Quaternion.identity);

            // Increase total number of enemies in the game
            currentEnemies++;
        }
	}
}
