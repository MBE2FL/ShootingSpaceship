using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayBar : MonoBehaviour
{
    [SerializeField]
    private Color currentColour;
    [SerializeField]
    private Color fullColour;
    [SerializeField]
    private Color halfColour;
    [SerializeField]
    private Color emptyColour;

    private Slider slider;


    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    // Use this for initialization
    void Start ()
    {
        currentColour = fullColour;
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void UpdateColour()
    {
        if (slider.value > 0.5f)
            currentColour = Color.Lerp(fullColour, halfColour, ((slider.value - 0.5f) / 0.5f));
    }

}
