using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shield : InventoryItem
{
    [Header("Shield Settings")]
    [SerializeField]
    [Range(25, 200)]
    private int energy;
    [SerializeField]
    [Range(25, 200)]
    private int maxEnergy;
    [SerializeField]
    [Range(0, 20)]
    private int absorbEnergyLoss;
    [SerializeField]
    [Range(0, 40)]
    private int defelctEnergyLoss;
    [SerializeField]
    [Range(1, 10)]
    private int energyDrainRate;
    [SerializeField]
    [Range(1, 10)]
    private int energyRestoreRate;
    [SerializeField]
    [Range(0, 10)]
    private float energyDrainDelay;
    [SerializeField]
    [Range(0, 15)]
    private float energyRestoreDelay;
    [SerializeField]
    [Range(10, 50)]
    private int maxBlock;
    public enum Mode { Absorb, Deflect };
    [SerializeField]
    private Mode currentMode;
    private Player player;
    private bool broken = false;


    // Visual Stuff
    private Renderer rend;
    [SerializeField]
    private Color currentColour;
    [SerializeField]
    private Color originalColour;
    [SerializeField]
    private Color breakColour;
    [SerializeField]
    private Color hitColour;
    private IEnumerator hitColorCoroutine;


    private IEnumerator drainCoroutine;
    private IEnumerator restoreCoroutine;

    private Transform field;


    // UI stuff
    private Camera cam;
    private RectTransform crosshair;
    private Text ammoText;
    private string ammoModeText;
    private Slider energyBar;

    // To disable update, while not currently in use
    private bool equipped = false;


    public Mode CurrentMode
    {
        get
        {
            return currentMode;
        }

        set
        {
            currentMode = value;
        }
    }


    private void Awake()
    {
        cam = Camera.main;
        crosshair = GameObject.Find("Crosshair").GetComponentInChildren<RectTransform>();
        ammoText = GameObject.Find("Ammo").GetComponent<Text>();
        energyBar = GameObject.Find("Energy Bar").GetComponent<Slider>();

        rend = transform.Find("Field").GetComponent<Renderer>();

        field = transform.Find("Field").GetComponent<Transform>();
    }

    // Use this for initialization
    void Start()
    {
        // Initialize UI variables
        Cursor.visible = false;
        energyBar.minValue = 0;
        energyBar.maxValue = maxEnergy;


        ammoModeText = "Energy: " + absorbEnergyLoss;

        // Shield will start in absorb mode
        currentMode = Mode.Absorb;

        player = GetComponentInParent<Player>();

        InvokeRepeating("ValidateVariables", 0.2f, 0.2f);

        
        originalColour = rend.material.color;
        currentColour = originalColour;
        breakColour = Color.red;
        breakColour.a = 0.5f;
        hitColour = Color.yellow;
        hitColour.a = 0.5f;
        hitColorCoroutine = HitShield();

        drainCoroutine = DrainEnergy();
        restoreCoroutine = RestoreEnergy(); 
    }

    private void OnValidate()
    {
        originalColour.a = 0.5f;
        breakColour.a = 0.5f;
        hitColour.a = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (equipped)
        {
            SwitchModes();

            // Update ammo text
            ammoText.text = ammoModeText + "/" + energy;

            // Break this shield, when it completely runs out of energy
            if ((energy <= 0) && !broken)
                BreakShield();
            // Restore this shield, when it regains it's max energy
            else if ((energy >= maxEnergy) && broken)
                RestoreShield();
        }

        energyBar.value = energy;
    }


    /// <summary>
    /// Absorbs damage which would normally be inflicted on the player.
    /// </summary>
    /// <param name="damage">The damage inflicted on this shield.</param>
    public void Ability(int damage)
    {
        // Absorb only works will this shield has enough energy, and is not broken
        if ((energy > absorbEnergyLoss) && !broken)
        {
            StartCoroutine(hitColorCoroutine);
            // If damage exceeds the max block of this shield, inflict the difference to the player
            if (damage > maxBlock)
            {
                // Inflict excess damage to player
                int diff = damage - maxBlock;
                player.loseHealth(diff);
            }

            // Lose energy for absorbing damage
            energy -= absorbEnergyLoss;
        }
        // Without enough energy the player will receive the full damage
        else
        {
            player.loseHealth(damage);
        }
    }

    /// <summary>
    /// Deflects this bullet back in the direction it was shot from.
    /// </summary>
    /// <param name="bulletRB">The rigidbody belonging to the bullet.</param>
    /// <param name="enemyPos">The enemy position to deflect the bullet back to.</param>
    /// <param name="damage">The damage inflicted on this shield.</param>
    public void Ability(Rigidbody bulletRB, Vector3 enemyPos, int damage)
    {
        // Deflect only works will this shield has enough energy, and is not broken
        if ((energy > defelctEnergyLoss) && !broken)
        {
            StartCoroutine(hitColorCoroutine);
            // Find the direction to deflect the bullet at
            Vector3 direction = (enemyPos - field.position).normalized;

            // Cancel the bullet's current velocity, and deflect in the direction of the enemy who shot it
            bulletRB.velocity = Vector3.zero;
            bulletRB.AddForce(direction * 4000.0f);

            // Lose energy for deflecting damage
            energy -= defelctEnergyLoss;
        }
        // Without enough energy the player will receive the full damage
        else
        {
            player.loseHealth(damage);
        }
    }

    /// <summary>
    /// Switch the shield between absorb or deflect mode.
    /// </summary>
    private void SwitchModes()
    {
        // Switch between absorbing and deflecting, with the middle mouse button
        if (Input.GetMouseButtonDown(2))
        {
            // Switch between absorbing and deflecting
            if (CurrentMode == Mode.Deflect)
            {
                currentMode = Mode.Absorb;
                ammoModeText = "Energy: " + absorbEnergyLoss;
            }
            else
            {
                currentMode = Mode.Deflect;
                ammoModeText = "Energy: " + defelctEnergyLoss;
            }
        }
    }

    IEnumerator DrainEnergy()
    {
        while (equipped)
        {
            // Lose energy while in use
            energy -= energyDrainRate;
            yield return new WaitForSeconds(1.0f);
        }

        Debug.Log("Drain Done");
        //drainCoroutine = DrainEnergy();
    }

    IEnumerator RestoreEnergy()
    {
        while (!equipped || broken)
        {
            // Restore energy while not in use
            energy += energyRestoreRate;
            yield return new WaitForSeconds(1.0f);
        }

        Debug.Log("Restore Done");
        //restoreCoroutine = RestoreEnergy();
    }

    /// <summary>
    /// Break this shield after it has exhausted all of it's energy.
    /// </summary>
    void BreakShield()
    {
        broken = true;

        // Stop potential change to hit colour
        StopCoroutine(hitColorCoroutine);
        hitColorCoroutine = HitShield();

        // Stop draining energy and start restoring
        StopCoroutine(drainCoroutine);
        StartCoroutine(restoreCoroutine);

        // Turn shield to red
        rend.material.SetColor("_Color", breakColour);
    }

    /// <summary>
    /// Restore this shield once it has regained it's max energy.
    /// </summary>
    void RestoreShield()
    {
        broken = false;

        // Start draining energy if equipped
        if (equipped)
        {
            StartCoroutine(drainCoroutine);
        }

        // Stop restoring energy
        StopCoroutine(restoreCoroutine);

        // Return shield to it's original colour
        rend.material.SetColor("_Color", originalColour);
    }

    IEnumerator HitShield()
    {
        // Smoothly change this shields colour to the specified hit colour
        float timeElapsed = 0.0f;
        float totalTime = 0.1f;

        while (timeElapsed <= totalTime)
        {
            timeElapsed += Time.deltaTime;
            currentColour = Color.Lerp(currentColour, hitColour, timeElapsed / totalTime);
            rend.material.SetColor("_Color", currentColour);
            yield return null;
        }

        currentColour = hitColour;
        rend.material.SetColor("_Color", currentColour);


        // Smoothly change this shields colour to the specified original colour
        timeElapsed = 0.0f;
        totalTime = 0.1f;

        while (timeElapsed <= totalTime)
        {
            timeElapsed += Time.deltaTime;
            currentColour = Color.Lerp(currentColour, originalColour, timeElapsed / totalTime);
            rend.material.SetColor("_Color", currentColour);
            yield return null;
        }

        currentColour = originalColour;
        rend.material.SetColor("_Color", currentColour);

        hitColorCoroutine = HitShield();
    }

    /// <summary>
    /// Ensures variables are in an acceptable range.
    /// </summary>
    void ValidateVariables()
    {
        energy = Mathf.Clamp(energy, 0, maxEnergy);
    }

    public override void Deactivate()
    {
        equipped = false;

        // While not broken, restore energy and stop draining while not equipped
        if (!broken)
        {
            StartCoroutine(restoreCoroutine);
            StopCoroutine(drainCoroutine);
        }

        // Turn crosshair off
        if (crosshair.gameObject.activeSelf)
            crosshair.gameObject.SetActive(false);

        // Disable all child gameobjects of this weapon
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    public override void Activate()
    {
        equipped = true;

        // While not broken, drain energy and stop restoring while equipped
        if (!broken)
        {
            StartCoroutine(drainCoroutine);
            StopCoroutine(restoreCoroutine);
        }

        // Turn crosshair off
        if (crosshair.gameObject.activeSelf)
            crosshair.gameObject.SetActive(false);

        // Enable all child gameobjects of this weapon
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }
}
