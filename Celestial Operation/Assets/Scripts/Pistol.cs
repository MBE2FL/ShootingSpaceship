using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pistol : Weapon
{
    [SerializeField]
    private Rigidbody bullet;
    private Transform bulletSpawn;

    private ParticleSystem muzzleFlash;


    private void Awake()
    {
        muzzleFlash = transform.Find("Muzzle Flash").GetComponent<ParticleSystem>();

        // Find sight object, for aiming down the sights
        sight = transform.Find("Sight").GetComponent<Transform>();
    }

    // Use this for initialization
    public override void Start ()
    {
        base.Start();
        bulletSpawn = transform.Find("BulletSpawn");
    }

    public override void PrimaryFire(Vector3 mousePos)
    {
        // Play muzzle flash particle system
        muzzleFlash.Play();

        // Change transparency of this weapon when aiming down the sights
        if (aimDown)
        {
            // Make gun partially transparent
        }
        else
        {
            // Make gun fully opaque
        }

        // Cast a ray from the centre of the viewport, though the camera, into the world
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

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
        //Rigidbody currentBullet = Instantiate(bullet, bulletSpawn.position, bulletRotation);
        GameObject currentBullet = ObjectPooler.Instance.SpawnObject("Bullet", bulletSpawn.position, bulletRotation);
        BulletLogic bulletLogic = currentBullet.GetComponent<BulletLogic>();
        bulletLogic.Damage = damage;
        bulletLogic.applyForce(bulletDirection * 4000.0f);
        bulletLogic.OwnerTransform = gameObject.GetComponentInParent<Transform>();
        ammo--;
    }

    public override void AltFire(Vector3 mousePos)
    {
        if (ammo >= shotsPerAlt)
        {
            // Mouse position on the screen (In pixels)
            //Vector3 mousePos = Input.mousePosition;

            // Play muzzle flash particle system
            muzzleFlash.Play();

            // Change transparency of this weapon when aiming down the sights
            if (aimDown)
            {
                // Make gun partially transparent
            }
            else
            {
                // Make gun fully opaque
            }

            // Cast a ray from the centre of the viewport, though the camera, into the world
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));


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
            for (int i = 0; i < shotsPerAlt; i++)
            {
                //Rigidbody currentBullet = Instantiate(bullet, bulletSpawn.position, bulletRotation);
                GameObject currentBullet = ObjectPooler.Instance.SpawnObject("Bullet", bulletSpawn.position, bulletRotation);
                BulletLogic bulletLogic = currentBullet.GetComponent<BulletLogic>();
                bulletLogic.Damage = damage;
                bulletLogic.applyForce(bulletDirection * 4000.0f);
                bulletLogic.OwnerTransform = gameObject.GetComponentInParent<Transform>();
                ammo--;
            }
        }
    }

}
