using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField]
    protected int ammo;
    [SerializeField]
    private string name;
    [SerializeField]
    protected int damage;
    [SerializeField]
    protected float fireRate;
    [SerializeField]
    private List<Enemy> extraDamageTypes;

    protected float startReloadTime;
    protected float passedReloadTime;
    protected bool reloaded = true;

    protected Camera cam;
    protected RectTransform crosshair;
    protected Text ammoText;

	// Use this for initialization
	public virtual void Start ()
    {
        // Start the gun off with a full clip
        startReloadTime = Time.time;
        passedReloadTime = 0.0f;

        cam = Camera.main;

        Cursor.visible = false;
        crosshair = GameObject.Find("Crosshair").GetComponentInChildren<RectTransform>();
        ammoText = GameObject.Find("Ammo").GetComponent<Text>();
    }
	
	// Update is called once per frame
	public void Update ()
    {
        Reload();


        // Mouse position on the screen (In pixels)
        Vector3 mousePos = Input.mousePosition;

        //RaycastHit hit;
        //Ray ray = Camera.main.ScreenPointToRay(mousePos);

        //if (Physics.Raycast(ray, out hit))
        //{
        //    Vector3 targetPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, hit.distance));

        //    transform.LookAt(targetPos);
        //}

        Vector3 targetPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 20.0f));
        transform.LookAt(targetPos);




        // Move crosshair with mouse
        crosshair.position = mousePos;


        // Update ammo text
        ammoText.text = "Ammo: " + ammo;
    }

    public abstract void PrimaryFire();

    public abstract void AltFire();

    void Reload()
    {
        //Debug.Log("start: " + startReloadTime);
        //Debug.Log("passed: " + passedReloadTime);

        if (!reloaded)
        {
            // After waiting a set time, reload the gun
            if (passedReloadTime >= fireRate)
            {
                reloaded = true;
                passedReloadTime = 0.0f;
                startReloadTime = Time.time;
            }
            // Track time till this weapon is reloaded
            else if (passedReloadTime < fireRate)
            {
                passedReloadTime = Time.time - startReloadTime;
            }
        }


        // Shoot this weapon, as long as it is reloaded, and has ammo
        if ((reloaded) && (ammo > 0))
        {
            // Use primary fire
            if (Input.GetMouseButtonDown(0))
            {
                PrimaryFire();
            }
            // Use altternate fire
            else if (Input.GetMouseButtonDown(1))
            {
                AltFire();
            }
        }
    }
}
