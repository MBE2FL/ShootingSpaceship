using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pistol : Weapon
{
    [SerializeField]
    private Rigidbody bullet;
    private Transform bulletSpawn;

    // Use this for initialization
    public override void Start ()
    {
        // Start the gun off with a full clip
        startReloadTime = Time.time;
        passedReloadTime = 0.0f;

        bulletSpawn = transform.Find("BulletSpawn");
        cam = Camera.main;

        Cursor.visible = false;
        crosshair = GameObject.Find("Crosshair").GetComponentInChildren<RectTransform>();
        ammoText = GameObject.Find("Ammo").GetComponent<Text>();
    }

    public override void PrimaryFire()
    {
        // Mouse position on the screen (In pixels)
        Vector3 mousePos = Input.mousePosition;

        // Cast a ray from the mouse position though the camera, into the world
        Ray ray = cam.ScreenPointToRay(mousePos);

        // Shoot the bullet in the same direction as the ray. (slighltly inaccurate, but only used when ray does not hit anything)
        Vector3 bulletDirection = ray.direction;

        // If the ray does hit something, find the direction from the bullet spawn to the point of impact. (Very accurate)
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            bulletDirection = (hit.point - bulletSpawn.position).normalized;
        }

        // Make sure bullet is always facing the way it will shoot towards
        Quaternion bulletRotation = Quaternion.FromToRotation(bullet.transform.forward, bulletDirection);

        /* Spawn a bullet, along with it's necessary information.
         * Lose one ammunition.
         */
        Rigidbody currentBullet = Instantiate(bullet, bulletSpawn.position, bulletRotation);
        currentBullet.GetComponent<BulletLogic>().Damage = damage;
        currentBullet.GetComponent<BulletLogic>().applyForce(bulletDirection * 4000.0f);
        ammo--;

        // Reload this pistol
        reloaded = false;
    }

    public override void AltFire()
    {
        if (ammo >= 3)
        {
            // Mouse position on the screen (In pixels)
            Vector3 mousePos = Input.mousePosition;

            // Cast a ray from the mouse position though the camera, into the world
            Ray ray = cam.ScreenPointToRay(mousePos);

            // Shoot the bullet in the same direction as the ray. (slighltly inaccurate, but only used when ray does not hit anything)
            Vector3 bulletDirection = ray.direction;

            // If the ray does hit something, find the direction from the bullet spawn to the point of impact. (Very accurate)
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                bulletDirection = (hit.point - bulletSpawn.position).normalized;
            }

            // Make sure bullet is always facing the way it will shoot towards
            Quaternion bulletRotation = Quaternion.FromToRotation(bullet.transform.forward, bulletDirection);

            /* Spawn a three bullets, along with their necessary information
             * Lose three ammunition.
             */
            for (int i = 0; i < 3; i++)
            {
                Rigidbody currentBullet = Instantiate(bullet, bulletSpawn.position, bulletRotation);
                currentBullet.GetComponent<BulletLogic>().Damage = damage;
                currentBullet.GetComponent<BulletLogic>().applyForce(bulletDirection * 4000.0f);
                ammo--;
            }

            // Reload this pistol
            reloaded = false;
        }
    }

}
