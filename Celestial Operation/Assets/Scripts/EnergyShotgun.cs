using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyShotgun : Weapon
{

    private Transform bulletSpawn;
    private Transform bulletSpawnTwo;

    private void Awake()
    {
        bulletSpawn = transform.Find("BulletSpawn");
        bulletSpawn = transform.Find("BulletSpawnTwo");
    }

    // Use this for initialization
    public override void Start ()
    {
        base.Start();
    }

    public override void PrimaryFire(Vector3 mousePos)
    {
        throw new System.NotImplementedException();
    }

    public override void AltFire(Vector3 mousePos)
    {
        throw new System.NotImplementedException();
    }

}
