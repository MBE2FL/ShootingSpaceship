using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatText : MonoBehaviour
{

    private string message;
    [SerializeField]
    private float lifeTime;
    [SerializeField]
    private float targetHeight;
    private Vector3 targetPos;
    private float elapsedTime;
    private TextMesh textMesh;
    private Vector3 startPos;
    private float totalTime;

    public string Message
    {
        get
        {
            return message;
        }

        set
        {
            message = value;
            textMesh.text = message;
        }
    }

    private void Awake()
    {
        textMesh = GetComponent<TextMesh>();
    }

    private void OnEnable()
    {
        // Reset float text, after being recyled through it's memory pool
        //lifeTime += Time.time;
        totalTime = lifeTime + Time.time;
        targetPos = transform.position;
        targetPos.y = targetHeight;
        startPos = transform.position;
    }
	
	// Update is called once per frame
	void Update ()
    {
        elapsedTime = Time.time;
        transform.position = Vector3.Lerp(startPos, targetPos, (elapsedTime / totalTime));

        if (elapsedTime >= totalTime)
            gameObject.SetActive(false);
    }
}
