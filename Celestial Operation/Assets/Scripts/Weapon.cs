using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Weapon : InventoryItem
{
    // General weapon stuff
    [SerializeField]
    protected int ammo;
    [SerializeField]
    private string name;
    [SerializeField]
    protected int damage;
    [SerializeField]
    protected int primeDamage;
    [SerializeField]
    protected int altDamage;
    [SerializeField]
    protected float fireRate;
    [SerializeField]
    protected float primeFireRate;
    [SerializeField]
    protected float altFireRate;
    [SerializeField]
    private List<Enemy> extraDamageTypes;
    [SerializeField]
    protected bool primeFire = true;        // Start this weapon in primary fire mode
    [SerializeField]
    protected int shotsPerPri;
    [SerializeField]
    protected int shotsPerAlt;


    // Reload stuff
    protected float reloadTimer = 0.0f;

    // UI stuff
    protected Camera cam;
    protected RectTransform crosshair;
    protected Text ammoText;
    protected string ammoModeText;

    // Sights stuff
    protected Transform sight;
    protected bool aimDown = false;
    [SerializeField]
    private float aimDownSpeed = 2.0f;
    private Vector3 originalPos;
    [SerializeField]
    private float weaponSmoothDamp = 6.0f;

    // To disable update, while not currently in use
    protected bool equipped = false;


    // Use this for initialization
    public virtual void Start()
    {
        Init();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (equipped)
        {
            /* Mouse position on the screen (In pixels)
             * Clamp mouse position, so the user cannot aim too close to the screen border
             */
            Vector3 mousePos = Input.mousePosition;
            mousePos.x = Mathf.Clamp(mousePos.x, 50.0f, (Screen.width - 50.0f));
            mousePos.y = Mathf.Clamp(mousePos.y, 50.0f, (Screen.height - 50.0f));


            // Shoot this weapon
            Shoot(mousePos);

            // Switch between primary fire and alternate fire
            SwitchModes();

            // Switch between using the crosshair or the sights to aim
            AimModes(mousePos);

            // Move crosshair with mouse
            //crosshair.position = mousePos;

            // Update ammo text
            ammoText.text = ammoModeText + "/" + ammo;
        }
    }

    private void Init()
    {
        // Initialize damage and fire rate
        damage = primeDamage;
        fireRate = primeFireRate;

        // Initialize UI variables
        cam = Camera.main;
        Cursor.visible = false;
        crosshair = GameObject.Find("Crosshair").GetComponentInChildren<RectTransform>();
        ammoText = GameObject.Find("Ammo").GetComponent<Text>();
        ammoModeText = "PRI Ammo: " + shotsPerPri;

        // Store orginal position of this weapon, to move back to after aiming down the sights
        originalPos = transform.localPosition;
    }

    public abstract void PrimaryFire(Vector3 mousePos);

    public abstract void AltFire(Vector3 mousePos);

    public virtual void Shoot(Vector3 mousePos)
    {
        // Shoot this weapon, as long as it is reloaded, and has ammo
        if ((Time.time >= reloadTimer) && (ammo > 0))
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Use primary fire
                if (primeFire)
                {
                    PrimaryFire(mousePos);
                }
                // Use alternate fire
                else
                {
                    AltFire(mousePos);
                }

                // Reset reload timer.
                reloadTimer = Time.time + fireRate;
            }
        }
    }


    public virtual void SwitchModes()
    {
        // Switch between primary fire and alternate fire, with the middle mouse button
        if (Input.GetMouseButtonDown(2))
        {
            primeFire = !primeFire;

            // Switch damage and fire rate to corresponding mode
            if (primeFire)
            {
                damage = primeDamage;
                fireRate = primeFireRate;
                ammoModeText = "PRI Ammo: " + shotsPerPri;
            }
            else
            {
                damage = altDamage;
                fireRate = altFireRate;
                ammoModeText = "ALT Ammo: " + shotsPerAlt;
            }
        }
    }


    void AimModes(Vector3 mousePos)
    {
        // Aim down the sights, while holding down the right mouse button
        if (Input.GetMouseButton(1))
        {
            // Disable crosshair, while aiming down the sights
            aimDown = true;
            crosshair.gameObject.SetActive(false);

            // Move gun with camera (Aim down the sights)
            // Find the position to move this weapon to, in order to be in the centre of the camera
            float camDistance = Vector3.Distance(sight.transform.position, cam.transform.position);
            Vector3 targetPos = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, camDistance));


            //Vector3 sightSize = sight.gameObject.GetComponent<Renderer>().bounds.size;
            //targetPos = new Vector3(targetPos.x - sightSize.x, targetPos.y - sightSize.y, targetPos.z);


            // Offset from the middle of this weapon and it's sight. (So sight is in the centre, instead of this weapon)
            Vector3 offset = sight.transform.position - transform.position;

            // Move this weapon to the centre of the camera
            transform.position = Vector3.Lerp(transform.position, (targetPos - offset), 0.5f);

            // Make sure it is always lined up with the camera
            transform.rotation = Quaternion.LookRotation(cam.transform.forward);
        }
        else
        {
            // Enable crosshair, while not aiming down the sights
            aimDown = false;
            crosshair.gameObject.SetActive(true);


            // Move gun back to normal position, after aiming down the sights
            if (Vector3.SqrMagnitude(transform.position - originalPos) > 0.5f)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, originalPos, weaponSmoothDamp * Time.deltaTime);
            }
            else
            {
                // Reset position after lerping for a while
                transform.localPosition = originalPos;
            }
        }
    }

    /// <summary>
    /// Hide this weapon, while it is not currently equipped.
    /// </summary>
    public override void Deactivate()
    {
        equipped = false;

        // Turn crosshair off
        if (crosshair.gameObject.activeSelf)
            crosshair.gameObject.SetActive(false);

        // Disable all child gameobjects of this weapon
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Show this weapon, while it is currently equipped.
    /// </summary>
    public override void Activate()
    {
        equipped = true;

        // Turn crosshair on
        if (!crosshair.gameObject.activeSelf)
            crosshair.gameObject.SetActive(true);

        // Enable all child gameobjects of this weapon
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }
}
