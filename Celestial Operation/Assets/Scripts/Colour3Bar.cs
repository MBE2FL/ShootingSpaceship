using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Colour3Bar : MonoBehaviour
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
    private Image fill;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        fill = transform.Find("Fill Area").Find("Fill").GetComponent<Image>();
    }

    // Use this for initialization
    void Start ()
    {
        currentColour = fullColour;
        fill.color = currentColour;
    }


    public void UpdateColour()
    {
        // Change colour between full and half
        if (slider.value > 50.0f)
            currentColour = Color.Lerp(halfColour, fullColour, ((slider.value - 50.0f) / 50.0f));
        // Change colour betweeen half and empty
        else
            currentColour = Color.Lerp(emptyColour, halfColour, (slider.value / 50.0f));

        // Update the colour of the fill
        fill.color = currentColour;

    }

}
