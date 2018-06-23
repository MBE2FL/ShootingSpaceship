using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
 
    private IEnumerator generateField;
    [SerializeField]
    private float fieldForce;
    private Rigidbody rb;
    [SerializeField]
    private float lifeTime;
    private float totalTime;
    private float elapsedTime;

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

        generateField = GenerateField();
        StartCoroutine(generateField);
    }

    // Use this for initialization
    void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    IEnumerator GenerateField()
    {
        elapsedTime = Time.time;

        while (elapsedTime <= totalTime)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 10.0f);

            foreach (Collider collider in colliders)
            {
                if (collider.tag == "Enemy")
                {
                    Vector3 posVec = transform.position - collider.GetComponent<Transform>().position;
                    float distance = Mathf.Pow(posVec.magnitude, 3);
                    Vector3 direction = posVec.normalized;
                    Vector3 force = ((500.0f * 1.0f) / distance) * direction;
                    collider.GetComponent<Rigidbody>().AddForce(force);

                    Debug.Log(force);
                }
            }
            yield return null;
        }

    }
}
