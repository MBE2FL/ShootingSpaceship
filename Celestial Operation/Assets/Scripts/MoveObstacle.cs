using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObstacle : MonoBehaviour {

    public Vector3 leftPoint;
    public Vector3 rightPoint;

    private Vector3 targetPosition;
    private Vector3 currPoint;
    private bool isLeft;

    // Use this for initialization
    void Start () {
        // Randomly choose which point to move to first
        int randPoint = (int)Random.Range(0.0f, 1.0f);

        if (randPoint == 0)
        {
            currPoint = leftPoint;
            isLeft = true;
        }
        else
        {
            currPoint = rightPoint;
            isLeft = false;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Vector3.Distance(currPoint, transform.position) <= 2.0f)
        {
            if (isLeft)
            {
                currPoint = rightPoint;
                isLeft = false;
            }
            else
            {
                currPoint = leftPoint;
                isLeft = true;
            }
        }

        targetPosition = Vector3.Lerp(transform.position, currPoint, 0.7f * Time.deltaTime);
        transform.position = targetPosition;
	}
}
