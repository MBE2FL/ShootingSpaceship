using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
 
    private IEnumerator generateField;
    private Rigidbody rb;
    [SerializeField]
    private float lifeTime;
    private float halfTime;
    [SerializeField]
    private Vector3 maxScale;
    private IEnumerator growShrink;
    [SerializeField]
    private float minMass;
    [SerializeField]
    private float maxMass;

    [SerializeField]
    private int maxDamage;
    [SerializeField]
    private float maxRadiusOfInfluence;
    private float currRadiusOfInfluence;

    private float radius;
    /// <summary>
    /// Gravitational constant
    /// </summary>
    private float G = 0.00000000006673f;
    /// <summary>
    /// Speed of light
    /// </summary>
    private float c = 299792458.0f;

    private Renderer rend;

    [SerializeField]
    private float attackDelay;

    public int MaxDamage
    {
        get
        {
            return maxDamage;
        }

        set
        {
            maxDamage = value;
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
    }

    private void OnEnable()
    {
        // Current radius of influence of this black hole
        currRadiusOfInfluence = 1.0f;

        // Current radius of this black hole
        //radius = rend.bounds.extents.magnitude;

        // Calculate half the life time of this black hole
        halfTime = lifeTime * 0.5f;

        // Start generating a gravitational field
        generateField = GenerateField();
        StartCoroutine(generateField);

        // Start to grow, and then shrink
        growShrink = GrowShrink();
        StartCoroutine(growShrink);
    }

    // Use this for initialization
    void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
        // Current radius of this black hole
        //radius = rend.bounds.extents.magnitude;
        //Debug.Log(radius);

        // Calculate current mass of this black hole
        //rb.mass = (Mathf.Pow(c, 2.0f) / (2 * G)) * radius;
    }

    /// <summary>
    /// Generates a gravitational field.
    /// Formula used F = ((M * m) / r^3) * r.
    /// Formula derived from F = ((G * M * m) / (r^3)) * r.
    /// </summary>
    /// <returns></returns>
    IEnumerator GenerateField()
    {
        // Reset the elapsed time of this black hole
        float elapsedTime = 0.0f;

        // Black hole will attack as soon as object comes into it's radius
        float elapsedAtkTime = attackDelay;
        bool attacked = false;

        // Generate gravitational field for the duration of this black holes life time
        while (elapsedTime <= lifeTime)
        {
            // Find all objects, with colliders attacted, in the vacinity of this black hole
            Collider[] colliders = Physics.OverlapSphere(transform.position, currRadiusOfInfluence);

            // Pull in each specified collider
            foreach (Collider collider in colliders)
            {
                if (collider.tag == "Enemy")
                {
                    // Retrieve enemy info
                    Vector3 enemyPos = collider.GetComponent<Transform>().position;
                    Rigidbody enemyRB = collider.GetComponent<Rigidbody>();
                    EnemyAI enemyAI = collider.GetComponent<EnemyAI>();

                    // Vector from enemy centre to this black hole's centre
                    Vector3 posVec = transform.position - enemyPos;
                    // Distance between this black hole's centre and the enemy's centre
                    float distance = posVec.magnitude;
                    // The distance between them cubed
                    float cubeDistance = Mathf.Pow(distance, 3);
                    // The direction from the enemy's centre to this black hole's centre
                    Vector3 direction = posVec.normalized;
                    // Calculate the force, from above info, and apply it to the enemy
                    Vector3 force = ((rb.mass * enemyRB.mass) / cubeDistance) * direction;
                    enemyRB.AddForce(force);

                    // Inlfict damage after a desired delay
                    if (elapsedAtkTime >= attackDelay)
                    {
                        // Inflict damage on the enemy, based on it's distance from this black hole
                        // NOTE: As black hole's mass, and therefore radius of influence, shrinks it does less damage
                        int damage = MaxDamage - (int)(MaxDamage * Mathf.Clamp(distance / currRadiusOfInfluence, 0.0f, 1.0f));
                        enemyAI.loseHealth(damage);

                        attacked = true;
                    }


                    //Debug.Log(force);
                    //Debug.Log(currRadiusOfInfluence);
                    //Debug.Log(Mathf.CeilToInt(distance / currRadiusOfInfluence));
                    //Debug.Log(distance / currRadiusOfInfluence);
                    //Debug.Log(Mathf.Clamp(distance / currRadiusOfInfluence, 0.0f, 1.0f));
                }
            }

            // Keep track of how long this black hole as been active
            elapsedTime += Time.deltaTime;

            // Keep track of the current delay, since the previous attack
            elapsedAtkTime += Time.deltaTime;
            // Reset variables to prevent this black hole from attacking, until enough time has passed
            if (attacked)
            {
                elapsedAtkTime = 0.0f;
                attacked = false;
            }

            yield return null;
        }

    }

    /// <summary>
    /// Grows and shrinks this black hole.
    /// </summary>
    /// <returns></returns>
    IEnumerator GrowShrink()
    {
        // Reset the elapsed time of this black hole
        float elapsedTime = 0.0f;

        // The starting scale of this black hole
        Vector3 originalScale = transform.localScale;

        // Grow until this black hole reaches half of it's life time
        while (elapsedTime <= halfTime)
        {
            transform.localScale = Vector3.Lerp(originalScale, maxScale, (elapsedTime / halfTime));

            // Adjust the mass for the increase in size
            rb.mass = Mathf.Lerp(minMass, maxMass, (elapsedTime / halfTime));

            // Adjust current radius of influence, for the increase in size
            currRadiusOfInfluence = Mathf.Lerp(1.0f, maxRadiusOfInfluence, (elapsedTime / halfTime));

            // Keep track of how long this black hole as been active
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Reset the elapsed time of this black hole
        elapsedTime = 0.0f;

        // Shrink once this black hole reaches half of it's life time
        while (elapsedTime <= lifeTime)
        {
            transform.localScale = Vector3.Lerp(maxScale, originalScale, (elapsedTime / halfTime));

            // Adjust the mass for the decrease in size
            rb.mass = Mathf.Lerp(maxMass, minMass, (elapsedTime / halfTime));

            // Adjust current radius of influence, for the decrease in size
            currRadiusOfInfluence = Mathf.Lerp(maxRadiusOfInfluence, 1.0f, (elapsedTime / halfTime));

            // Keep track of how long this black hole as been active
            elapsedTime += Time.deltaTime;

            yield return null;
        }

    }

}
