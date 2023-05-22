using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FemaleZombieAController : ZombieController
{
    #region Behavour Variables
    private int routine;
    private float time;
    private Quaternion angle;
    private float grade;
    private float lastHitMelee = 0f;
    #endregion

    public override void ZombieBehavor()
    {
        if (Vector3.Distance(transform.position, zombieData.player.transform.position) > zombieData.distanceToChase)
        {
            animatorController.ZombieChasing(false);

            time += 1 * Time.deltaTime;
            if (time >= 4)
            {
                routine = Random.Range(0, 2);
                time = 0;
            }
            switch (routine)
            {
                case 0:
                    animatorController.ZombieWalk(false);
                    break;

                case 1:
                    grade = Random.Range(0, 360);
                    angle = Quaternion.Euler(0, grade, 0);
                    routine++;
                    break;

                case 2:
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, angle, zombieData.speedRotationWalk);
                    transform.Translate(Vector3.forward * zombieData.speedWalk * Time.deltaTime);
                    animatorController.ZombieWalk(true);
                    break;
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, zombieData.player.transform.position) > 1)
            {
                var lookPos = zombieData.player.transform.position - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, zombieData.speedRotationRun);

                animatorController.ZombieWalk(false);
                animatorController.ZombieChasing(true);

                transform.Translate(Vector3.forward * zombieData.speedRun * Time.deltaTime);

                animatorController.ZombieHitMelee(false);
            }
            else
            {
                animatorController.ZombieHitMelee(true);
                animatorController.ZombieWalk(false);
                animatorController.ZombieChasing(false);

                if (Time.time > lastHitMelee + 1.8f)
                {
                    HitMelee();
                    lastHitMelee = Time.time;
                }
            }

        }
    }
}
