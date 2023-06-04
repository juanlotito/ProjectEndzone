using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/ZombieData")]
public class ZombieData : ScriptableObject
{
    #region Patroll Variables
    public float speedWalk = 2f;
    public float speedRotationWalk = 0.5f;
    #endregion

    #region Chasing Variables
    public float speedRotationRun = 1f;
    public float speedRun = 2f;
    public float distanceToChase  = 15f;
    #endregion

    #region Melee Variables
    public float meleeRange  = 15f;
    public float reloadTime  = 15f;
    public float meleDamage  = 15f;
    #endregion

    public GameObject GetPlayer()
    {
        return PlayerManager.instance.player;
    }
}
