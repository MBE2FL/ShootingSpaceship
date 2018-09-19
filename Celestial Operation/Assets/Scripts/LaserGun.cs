using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : Weapon
{
    private Transform laser;
    private LineRenderer lineRend;
    private Light laserLight;
    [SerializeField]
    private float laserDuration;
    private float laserTimer;
    private Transform laserOrigin;
    [SerializeField]
    private float chargeTime;
    private float chargeTimer = 0.0f;
    [SerializeField]
    private float altTime;
    private float altTimer;
    [SerializeField]
    private float primeForce;
    [SerializeField]
    private float altForce;


    private IEnumerator altBeam;

    private void Awake()
    {
        laser = transform.Find("Laser");
        lineRend = laser.GetComponent<LineRenderer>();
        laserLight = laser.GetComponent<Light>();
        laserOrigin = transform.Find("Laser Origin");

        // Find sight object, for aiming down the sights
        sight = transform.Find("Sight").GetComponent<Transform>();
    }

    // Use this for initialization
    public override void Start()
    {
        base.Start();

        laserTimer = 0.0f;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        laserTimer += Time.deltaTime;

        if (primeFire && (laserTimer >= laserDuration * fireRate))
        {
            lineRend.enabled = false;
            laserLight.enabled = false;
        }
    }

    private void OnValidate()
    {
        // Make sure previous laser beams vanish before the player can shoot a new one.
        if (primeFire)
        {
            if (laserDuration >= primeFireRate)
            {
                laserDuration = primeFireRate - 0.1f;
            }
        }
        //else
        //{
        //    if (laserDuration >= altFireRate)
        //    {
        //        laserDuration = altFireRate - 0.1f;
        //    }
        //}

    }

    public override void PrimaryFire(Vector3 mousePos)
    {
        // Reset the timer.
        laserTimer = 0.0f;

        // Activate the line renderer and light. Then set the first position of the renderer to the barrel. 
        lineRend.enabled = true;
        laserLight.enabled = true;
        lineRend.SetPosition(0, laserOrigin.transform.position);

        // Cast a ray from the centre of the viewport, though the camera, into the world
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

        // Set the second position of the renderer to a point in the far distance, in case it does not hit anything.
        lineRend.SetPosition(1, ray.origin + ray.direction * 200.0f);

        // If the ray does hit something, set that point as the second position of the renderer.
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            lineRend.SetPosition(1, hit.point);

            // Damage any enemies hit by this weapon's laser.
            if (hit.transform.tag == "Enemy")
            {
                hit.transform.GetComponent<EnemyAI>().loseHealth(damage);
            }

            // Apply a force to any object, which has a rigidbody, hit by this weapon's laser.
            if (hit.rigidbody)
            {
                hit.rigidbody.AddForceAtPosition(ray.direction * primeForce, hit.point);
            }
        }

        // Lose ammunition.
        ammo--;
    }

    public override void AltFire(Vector3 mousePos)
    {
        if (Time.time > chargeTimer)
        {
            // Increase the size of the laser beam.
            lineRend.startWidth = 0.08f;
            lineRend.endWidth = 0.08f;

            // Activate the line renderer and light. Then set the first position of the renderer to the barrel. 
            lineRend.enabled = true;
            laserLight.enabled = true;


            altBeam = altHelper();
            StartCoroutine(altBeam);
        }
    }

    IEnumerator altHelper()
    {
        float altTimer = Time.time + altTime;

        while (Time.time <= altTimer)
        {
            lineRend.SetPosition(0, laserOrigin.transform.position);

            lineRend.material.mainTextureOffset = new Vector2(0, Time.time);

            // Cast a ray from the centre of the viewport, though the camera, into the world
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

            // Set the second position of the renderer to a point in the far distance, in case it does not hit anything.
            lineRend.SetPosition(1, ray.origin + ray.direction * 200.0f);

            // If the ray does hit something, set that point as the second position of the renderer.
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                lineRend.SetPosition(1, hit.point);

                // Damage any enemies hit by this weapon's laser.
                if (hit.transform.tag == "Enemy")
                {
                    hit.transform.GetComponent<EnemyAI>().loseHealth(damage);
                }

                // Apply a force to any object, which has a rigidbody, hit by this weapon's laser.
                if (hit.rigidbody)
                {
                    hit.rigidbody.AddForceAtPosition(ray.direction * altForce, hit.point);
                }
            }

            yield return null;
        }


        // Lose ammunition.
        ammo--;

        // Disable effects.
        lineRend.enabled = false;
        laserLight.enabled = false;

        // Reset the charge timer.
        chargeTimer = Time.time + chargeTime;
    }


    public override void Shoot(Vector3 mousePos)
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
                    reloadTimer = Time.time + fireRate;
                }
                // Use alternate fire
                else
                {
                    AltFire(mousePos);
                }
            }

        }
    }
}