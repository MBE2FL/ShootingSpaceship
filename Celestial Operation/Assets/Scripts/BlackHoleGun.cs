using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleGun : Weapon
{

    [SerializeField]
    private Rigidbody blackHole;
    private Transform bulletSpawn;
    private Rigidbody currentBlackHole;
    private IEnumerator activateBH;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        bulletSpawn = transform.Find("Black Hole Spawn");
        currentBlackHole = transform.Find("Black Hole").GetComponent<Rigidbody>();
    }

    public override void PrimaryFire(Vector3 mousePos)
    {
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
        Vector3 blackHoleDirection = ray.direction;

        // If the ray does hit something, find the direction from the bullet spawn to the point of impact. (Very accurate)
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            blackHoleDirection = (hit.point - bulletSpawn.position).normalized;
        }


        // Make sure bullet is always facing the way it will shoot towards
        Quaternion blackHoleRotation = Quaternion.FromToRotation(blackHole.transform.forward, blackHoleDirection);

        /* Spawn a bullet, along with it's necessary information.
         * Lose one ammunition.
         */
        //Rigidbody currentBlackHole = Instantiate(blackHole, bulletSpawn.position, blackHoleRotation);
        //GameObject currentBlackHole = ObjectPooler.Instance.SpawnObject("Black Hole", blackHole.position, blackHoleRotation);
        BlackHole blackHoleLogic = currentBlackHole.GetComponent<BlackHole>();
        blackHoleLogic.MaxDamage = damage;
        currentBlackHole.isKinematic = false;
        currentBlackHole.useGravity = true;
        currentBlackHole.transform.parent = null;
        currentBlackHole.AddForce(blackHoleDirection * 400.0f);
        activateBH = ActivateBlackHole();
        StartCoroutine(activateBH);
        ammo--;
    }

    public override void AltFire(Vector3 mousePos)
    {
        throw new System.NotImplementedException();
    }

    IEnumerator ActivateBlackHole()
    {
        float elapsedTime = 0.0f;
        float delay = Time.time + 0.5f;

        // After the current black hole's velocity is slow enough, activate it's ability
        while ((currentBlackHole.velocity.sqrMagnitude > 0.15f) || (elapsedTime < delay))
        {
            elapsedTime = Time.time;
            Debug.Log(currentBlackHole.velocity.sqrMagnitude);
            yield return null;
        }

        currentBlackHole.isKinematic = true;
        currentBlackHole.useGravity = false;
        currentBlackHole.GetComponent<BlackHole>().enabled = true;
    }

}
