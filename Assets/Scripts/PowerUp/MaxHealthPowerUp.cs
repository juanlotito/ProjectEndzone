using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHealthPowerUp : BasicPowerUpController
{
    public override void MakeEffect(FirstPersonController player)
    {
        player.healthSystem.HealthPowerUpPicked();
    }
}
