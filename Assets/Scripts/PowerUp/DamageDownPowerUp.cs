using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDownPowerUp : BasicPowerUpController
{
    public override void MakeEffect(FirstPersonController player)
    {
        player.OnDamagePowerDownPicked();
    }

}
