using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
 
    private IEnumerator generateField;
    private Rigidbody rb;
    [SerializeField]
    private float lifeTime;
    private float totalTime;
    private float elapsedTime;
    private float halfTime;
    [SerializeField]
    private Vector3 maxScale;
    private IEnumerator growShrink;

    private float radius;
    private float gravitationalConstant = 1.0f;
    private float speedOfLight = 200.0f;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        totalTime = lifeTime + Time.time;
        halfTime = (lifeTime * 0.5f) + Time.time;

        generateField = GenerateField();
        //StartCoroutine(generateField);

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
        elapsedTime = Time.time;
    }

    IEnumerator GenerateField()
    {
        // Generate gravitational field for the duration of this black holes life time
        while (elapsedTime <= totalTime)
        {
            // Find all objects, with colliders attacted, in the vacinity of this black hole
            Collider[] colliders = Physics.OverlapSphere(transform.position, 10.0f);

            // Pull in each collider
            foreach (Collider collider in colliders)
            {
                if (collider.tag == "Enemy")
                {
                    Vector3 posVec = transform.position - collider.GetComponent<Transform>().position;
                    float distance = Mathf.Pow(posVec.magnitude, 3);
                    Vector3 direction = posVec.normalized;
                    Vector3 force = ((500.0f * 1.0f) / distance) * direction;
                    collider.GetComponent<Rigidbody>().AddForce(force);

                    //Debug.Log(force);
                }
            }
            yield return null;
        }

    }

    IEnumerator GrowShrink()
    {
        Vector3 originalScale = transform.localScale;

        // Grow until this black hole reaches half of it's life time
        while (elapsedTime <= halfTime)
        {
            transform.localScale = Vector3.Lerp(originalScale, maxScale, (elapsedTime / halfTime));
            Debug.Log(transform.localScale);
            yield return null;
        }

        // Shrink once this black hole reaches half of it's life time
        while (elapsedTime <= totalTime)
        {
            transform.localScale = Vector3.Lerp(maxScale, originalScale, (elapsedTime / totalTime));
            Debug.Log(transform.localScale);
            yield return null;
        }

    }
}
