using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayBar : MonoBehaviour
{
    [SerializeField]
    private Color currentColour;

    [SerializeField]
    private List<Color> colours;

    private Slider slider;
    private Image fill;
    [SerializeField]
    private float maxValue;
    private float valuePerColour;
    private int numTransitions = 0;


    private void Awake()
    {
        slider = GetComponent<Slider>();
        fill = transform.Find("Fill Area").Find("Fill").GetComponent<Image>();
    }

    // Use this for initialization
    void Start ()
    {
        // At least one colour in the list
        if (colours.Count > 0)
        {
            // Set this bar's colour to the first colour in the list
            currentColour = colours[0];
            fill.color = currentColour;
            //maxValue = slider.maxValue;

            // At least two colours in the list
            if (colours.Count > 1)
            {
                // Number of colour transitions is the total number of colours minus one
                numTransitions = colours.Count - 1;
                // Divide the max value of the slider amongst all the transitions
                valuePerColour = maxValue / numTransitions;
            }
        }
	}

    /// <summary>
    /// Smoothly changes this bar's colour.
    /// </summary>
    public void UpdateColour()
    {
        /* Find the correct two colours to lerp between, based on the current value of the slider and
         * the number of colour transitions
         */
        for (int i = numTransitions; i > 0; i--)
        {
            // Value used to determine which two colours we are inbetween
            float colourValue = valuePerColour * (i - 1);

            // Inbetween last and second last colour
            if (colourValue <= 0)
            {
                currentColour = Color.Lerp(colours[i - 1], colours[i], (slider.value / valuePerColour));
                break;
            }
            // Inbetween two colours, non of which are the last colour
            else if (slider.value > colourValue)
            {
                currentColour = Color.Lerp(colours[i - 1], colours[i], ((slider.value - colourValue) / valuePerColour));
                break;
            }
        }

        // Update the colour of the fill
        fill.color = currentColour;

    }

}
