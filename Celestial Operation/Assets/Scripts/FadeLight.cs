using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeLight : MonoBehaviour
{
    private Light pointLight;

    private void Awake()
    {
        pointLight = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update ()
    {
        if (pointLight.range > 0.05f)
            pointLight.range = Mathf.Lerp(pointLight.range, 0.0f, Time.deltaTime);
	}
}
