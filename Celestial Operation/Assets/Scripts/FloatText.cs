﻿using System.Collections;
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
        elapsedTime = 0.0f;
        targetPos = transform.position;
        targetPos.y = targetHeight;
        startPos = transform.position;
    }
	
	// Update is called once per frame
	void Update ()
    {
        // Float from the start position to the target postion, in the specified time
        elapsedTime += Time.deltaTime;
        transform.position = Vector3.Lerp(startPos, targetPos, (elapsedTime / lifeTime));

        // Deactivate this float text, once it has reached it's maximum life time
        if (elapsedTime >= lifeTime)
            gameObject.SetActive(false);
    }
}
