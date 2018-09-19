using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private List<InventoryItem> items;
    [SerializeField]
    private InventoryItem currentItem;
    [SerializeField]
    private int numOfGrenades;
    private Transform grenadeSpawn;
    public Rigidbody grenade;
    private Camera cam;


    private void Awake()
    {
        cam = Camera.main;
        grenadeSpawn = cam.transform.Find("Grenade Spawn");
        //grenade = Resources.Load("Prefabs/Grenade") as GameObject;
    }

    // Use this for initialization
    void Start ()
    {
        // Start with item in the first item slot
        currentItem = items[0];
        //currentItem.gameObject.SetActive(true);
        currentItem.Activate();

        // Make sure all other items are diabled, except for the first one
        for (int i = 1; i < items.Count; i++)
        {
            //items[i].gameObject.SetActive(false);
            items[i].Deactivate();
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        ScrollWeapons();

        updateGrenades();
    }

    private void ScrollWeapons()
    {
        // Scroll up, through the item slots
        if (Input.GetAxis("Mouse ScrollWheel") > 0.0f)
        {
            int currentSlot = items.IndexOf(currentItem);

            // At the first slot
            if (currentSlot == 0)
            {
                // Deactivate previous item, and activate new item
                currentItem.Deactivate();
                currentItem = items[items.Count - 1];
                currentItem.Activate();
            }
            // Not at the first slot
            else if ((currentSlot > 0) && (currentSlot < items.Count))
            {
                // Deactivate previous item, and activate new item
                currentItem.Deactivate();
                currentItem = items[currentSlot - 1];
                currentItem.Activate();
            }
            // At an invalid slot
            else if ((currentSlot < 0) || (currentSlot >= items.Count))
            {
                Debug.LogError("Invalid item slot in use!");
            }
        }
        // Scroll down, through the item slots
        else if (Input.GetAxis("Mouse ScrollWheel") < 0.0f)
        {
            int currentSlot = items.IndexOf(currentItem);

            // At the last slot
            if (currentSlot == (items.Count - 1))
            {
                // Deactivate previous item, and activate new item
                currentItem.Deactivate();
                currentItem = items[0];
                currentItem.Activate();
            }
            // Not at the last slot
            else if ((currentSlot >= 0) && (currentSlot < (items.Count - 1)))
            {
                // Deactivate previous item, and activate new item
                currentItem.Deactivate();
                currentItem = items[currentSlot + 1];
                currentItem.Activate();
            }
            // At an invalid slot
            else if ((currentSlot < 0) || (currentSlot >= items.Count))
            {
                Debug.LogError("Invalid item slot in use!");
            }
        }
    }

    void updateGrenades()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            numOfGrenades--;

            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

            Rigidbody currentGrenade = Instantiate(grenade, grenadeSpawn.position, grenadeSpawn.rotation);
            currentGrenade.AddForce(ray.direction * 200.0f);
        }
    }
}
