using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private List<Weapon> weapons;
    [SerializeField]
    private Weapon currentWeapon;

	// Use this for initialization
	void Start ()
    {
        // Start with weapon in the first weapon slot
        currentWeapon = weapons[0];
	}
	
	// Update is called once per frame
	void Update ()
    {
        ScrollWeapons();

    }

    private void ScrollWeapons()
    {
        // Scroll up, through the weapon slots
        if (Input.GetAxis("Mouse ScrollWheel") > 0.0f)
        {
            int currentSlot = weapons.IndexOf(currentWeapon);

            // At the first slot
            if (currentSlot == 0)
            {
                currentWeapon = weapons[weapons.Count];
            }
            // Not at the first slot
            else if ((currentSlot > 0) && (currentSlot <= weapons.Count))
            {
                currentWeapon = weapons[currentSlot - 1];
            }
            // At an invalid slot
            else if ((currentSlot < 0) || (currentSlot > weapons.Count))
            {
                Debug.LogError("Invalid weapon slot in use!");
            }
        }
        // Scroll down, through the weapon slots
        else if (Input.GetAxis("Mouse ScrollWheel") < 0.0f)
        {
            int currentSlot = weapons.IndexOf(currentWeapon);

            // At the last slot
            if (currentSlot == weapons.Count)
            {
                currentWeapon = weapons[0];
            }
            // Not at the last slot
            else if ((currentSlot >= 0) && (currentSlot < weapons.Count))
            {
                currentWeapon = weapons[currentSlot + 1];
            }
            // At an invalid slot
            else if ((currentSlot < 0) || (currentSlot > weapons.Count))
            {
                Debug.LogError("Invalid weapon slot in use!");
            }
        }
    }
}
