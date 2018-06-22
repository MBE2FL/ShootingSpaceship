using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{

    private Transform camPos;
    private float currentXRotation = 0.0f;
    private float currentYRotation = 0.0f;
    [SerializeField]
    [Range(-5.0f, -60.0f)]
    private float minXDegrees;
    [SerializeField]
    [Range(5.0f, 60.0f)]
    private float maxXDegrees;
    [SerializeField]
    [Range(-30.0f, -120.0f)]
    private float minYDegrees;
    [SerializeField]
    [Range(30.0f, 120.0f)]
    private float maxYDegrees;


    private void Awake()
    {
        camPos = GetComponent<Transform>();
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Move camera with the mouse
        /*
         * First rotate around local x-axis (pitch)
         * Then roate around global y-axis (yaw)
         * (Rotate around y-axis globally due to x-axis rotation moving local y-axis)
         */
        float horizontal = Input.GetAxis("Mouse X");
        float vertical = -Input.GetAxis("Mouse Y");


        // Clamp rotation around the x-axis
        if ((currentXRotation >= minXDegrees) && (currentXRotation <= maxXDegrees))
        {
            transform.Rotate(vertical, 0.0f, 0.0f);
        }
        // Clamp currentXRotation to allow for quick change in direction
        else
        {
            currentXRotation = Mathf.Clamp(currentXRotation, minXDegrees - 1.0f, maxXDegrees + 1.0f);
        }

        // Clamp rotation around the y-axis
        if ((currentYRotation >= minYDegrees) && (currentYRotation <= maxYDegrees))
        {
            transform.Rotate(0.0f, horizontal, 0.0f, Space.World);
        }
        // Clamp currentYRotation to allow for quick change in direction
        else
        {
            currentYRotation = Mathf.Clamp(currentYRotation, minYDegrees - 1.0f, maxYDegrees + 1.0f);
        }

        // Keep track of degrees rotated in each axis
        currentXRotation += vertical;
        currentYRotation += horizontal;
    }
}
