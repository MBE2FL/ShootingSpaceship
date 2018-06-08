using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleGun : Weapon
{

    [SerializeField]
    private Rigidbody blackholeDevice;
    private Transform bulletSpawn;

    // Use this for initialization
    public override void Start()
    {

    }

    public override void PrimaryFire(Vector3 mousePos)
    {
        Ray ray;

        // Shoot a ray through the centre of the camera when aiming down the sights, else shoot where the mouse is
        if (aimDown)
        {
            // Cast a ray from the centre of the viewport, though the camera, into the world
            ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
        }
        else
        {
            // Cast a ray from the mouse position though the camera, into the world
            ray = cam.ScreenPointToRay(mousePos);
        }


        // Shoot the bullet in the same direction as the ray. (slighltly inaccurate, but only used when ray does not hit anything)
        Vector3 bulletDirection = ray.direction;

        // If the ray does hit something, find the direction from the bullet spawn to the point of impact. (Very accurate)
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            bulletDirection = (hit.point - bulletSpawn.position).normalized;
        }


        // Make sure bullet is always facing the way it will shoot towards
        Quaternion bulletRotation = Quaternion.FromToRotation(blackholeDevice.transform.forward, bulletDirection);

        /* Spawn a bullet, along with it's necessary information.
         * Lose one ammunition.
         */
        Rigidbody currentBullet = Instantiate(blackholeDevice, bulletSpawn.position, bulletRotation);
        currentBullet.GetComponent<BulletLogic>().Damage = damage;
        currentBullet.GetComponent<BulletLogic>().applyForce(bulletDirection * 4000.0f);
        ammo--;

        // Reload this pistol
        reloaded = false;
    }

    public override void AltFire(Vector3 mousePos)
    {
        throw new System.NotImplementedException();
    }

}
