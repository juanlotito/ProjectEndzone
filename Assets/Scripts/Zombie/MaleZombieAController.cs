using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaleZombieAController : ZombieController
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
                animatorController.ZombieWalk(false);
                animatorController.ZombieChasing(true);

                Vector3 targetDirection = zombieData.player.transform.position - transform.position;
                targetDirection.y = 0;
                Quaternion rotation = Quaternion.LookRotation(targetDirection);

                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, zombieData.speedRotationRun * 2f);

                transform.Translate(Vector3.forward * zombieData.speedRun * 1.5f * Time.deltaTime);

                animatorController.ZombieAttackAmbush(true);

                if (Time.time > lastHitMelee + 1.8f)
                {
                    HitMelee();
                    lastHitMelee = Time.time;
                }
            }
            else
            {
                animatorController.ZombieHitMelee(true);
                animatorController.ZombieWalk(false);
                animatorController.ZombieChasing(false);
                animatorController.ZombieAttackAmbush(false);

                if (Time.time > lastHitMelee + 1.8f)
                {
                    HitMelee();
                    lastHitMelee = Time.time;
                }
            }
        }
    }

}
