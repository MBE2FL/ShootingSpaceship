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
    protected bool primeFire;
    [SerializeField]
    protected int shotsPerPri;
    [SerializeField]
    protected int shotsPerAlt;


    // Reload stuff
    protected float startReloadTime;
    protected float passedReloadTime;
    protected bool reloaded = true;

    // UI stuff
    protected Camera cam;
    protected RectTransform crosshair;
    protected Text ammoText;
    protected string ammoModeText;

    // Sights stuff
    private Transform sight;
    protected bool aimDown = false;
    [SerializeField]
    private float aimDownSpeed;
    private Quaternion originalRotation;
    private Vector3 originalPos;
    [SerializeField]
    private float camSmoothDamp = 6.0f;
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

            // Reload and shoot the gun when necessary
            Reload(mousePos);

            // Switch between primary fire and alternate fire
            SwitchModes();

            // Switch between using the crosshair or the sights to aim
            AimModes(mousePos);

            // Move crosshair with mouse
            crosshair.position = mousePos;

            // Update ammo text
            ammoText.text = ammoModeText + "/" + ammo;
        }
    }

    private void Init()
    {
        // Start the gun off fully reloaded
        startReloadTime = Time.time;
        passedReloadTime = 0.0f;

        // Initialize damage and fire rate
        damage = primeDamage;
        fireRate = primeFireRate;

        // Initialize UI variables
        cam = Camera.main;
        Cursor.visible = false;
        crosshair = GameObject.Find("Crosshair").GetComponentInChildren<RectTransform>();
        ammoText = GameObject.Find("Ammo").GetComponent<Text>();
        ammoModeText = "PRI Ammo: " + shotsPerPri;

        // Start this weapon in primary fire mode
        primeFire = true;

        // Find sight object, for aiming down the sights
        if (GameObject.Find("Sight"))
            sight = GameObject.Find("Sight").GetComponent<Transform>();

        // Store orginal rotation of the camera, to rotate back to after aiming down the sights
        originalRotation = cam.transform.rotation;
        originalPos = transform.position;
    }

    public abstract void PrimaryFire(Vector3 mousePos);

    public abstract void AltFire(Vector3 mousePos);

    void Reload(Vector3 mousePos)
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
            if (Input.GetMouseButtonDown(0))
            {
                // Use primary fire
                if (primeFire)
                {
                    PrimaryFire(mousePos);
                    //Debug.Log("PRIME start: " + startReloadTime);
                }
                // Use alternate fire
                else
                {
                    AltFire(mousePos);
                    //Debug.Log("ALT start: " + startReloadTime);
                }
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

            // Move camera with the mouse
            /*
             * First rotate around local x-axis (pitch)
             * Then roate around global y-axis (yaw)
             * (Rotate around y-axis globally due to x-axis rotation moving local y-axis)
             */
            float horizontal = Input.GetAxis("Mouse X");
            float vertical = -Input.GetAxis("Mouse Y");
            cam.transform.Rotate(vertical, 0.0f, 0.0f);
            cam.transform.Rotate(0.0f, horizontal, 0.0f, Space.World);


            // Clamp rotation of aiming down the sights
            Vector3 clampedRot = cam.transform.eulerAngles;

            // Clamp x degrees in [0, 20] union [340, 360]
            if (clampedRot.x > 20.0f && clampedRot.x < 160.0f)
            {
                clampedRot.x = 20.0f;
            }
            else if (clampedRot.x < 340.0f && clampedRot.x >= 160.0f)
            {
                clampedRot.x = 340.0f;
            }

            // Clamp y degrees in [0, 25] union [335, 360]
            if (clampedRot.y > 25.0f && clampedRot.y < 155.0f)
            {
                clampedRot.y = 25.0f;
            }
            else if (clampedRot.y < 335.0f && clampedRot.y >= 155.0f)
            {
                clampedRot.y = 335.0f;
            }

            cam.transform.eulerAngles = clampedRot;
        }
        else
        {
            // Enable crosshair, while not aiming down the sights
            aimDown = false;
            crosshair.gameObject.SetActive(true);

            // Rotate camera back to normal, after aiming down the sights
            if (Vector3.SqrMagnitude(cam.transform.eulerAngles - originalRotation.eulerAngles) > 0.5f)
            {
                cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, originalRotation, camSmoothDamp * Time.deltaTime);
            }
            else
            {
                // Reset rotation after slerping for a while
                cam.transform.rotation = originalRotation;
            }

            // Move gun back to normal position, after aiming down the sights
            if (Vector3.SqrMagnitude(transform.position - originalPos) > 0.5f)
            {
                transform.position = Vector3.Lerp(transform.position, originalPos, weaponSmoothDamp * Time.deltaTime);
            }
            else
            {
                // Reset position after lerping for a while
                transform.position = originalPos;

                // Move gun
                Vector3 targetPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 20.0f));
                targetPos = new Vector3(Mathf.Clamp(targetPos.x, -14.0f, 14.0f), Mathf.Clamp(targetPos.y, -7.0f, 7.0f), targetPos.z);
                transform.LookAt(targetPos);
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
