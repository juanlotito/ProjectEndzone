using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUpPowerUp : BasicPowerUpController
{
    public override void MakeEffect(FirstPersonController player)
    {
        player.OnDamagePowerUpPicked();
    }
}
