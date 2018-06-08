using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Weapon
{
    [SerializeField]
    private int energy;
    [SerializeField]
    private int maxBlock;
    private enum Mode { Absorb, Deflect };
    [SerializeField]
    private Mode currentMode;


    // Use this for initialization
    public override void Start()
    {
        // Shield will start in absorb mode
        currentMode = Mode.Absorb;
    }

    // Update is called once per frame
    public override void Update()
    {
        // Ensure shieldl is in the centre of the screen
        Vector3 centrePos = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        if (transform.position != centrePos)
        {
            transform.position = centrePos;
        }
    }


    public void Ability(int damage)
    {
        // Deal with the damage, depending on the current mode this shield is in
        switch (currentMode)
        {
            case Mode.Absorb:
                {
                    // If damage exceeds the max block of this shield, inflict the difference to the player
                    if (damage > maxBlock)
                    {

                    }
                    break;
                }
            case (Mode.Deflect):
                {
                    break;
                }
            default:
                {
                    Debug.LogError("Shield Abnormality");
                    break;
                }
        }

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
