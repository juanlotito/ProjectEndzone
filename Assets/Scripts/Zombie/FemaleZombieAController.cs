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
    private bool zombieCanMove = true;
    private Rigidbody[] rigidbodies;
    [SerializeField] private Animator animator;
    #endregion


    private void Start()
    {
        rigidbodies = transform.GetComponentsInChildren<Rigidbody>();
        SetEnabled(false);
        this.gameManager = GameManager.instance;
    }

    private void Awake()
    {
        this.healthSystem.OnEntityDead += OnEntityDeadHandler;
    }

    public override void ZombieBehavor()
    {
        if ((Vector3.Distance(transform.position, zombieData.GetPlayer().transform.position) > zombieData.distanceToChase) && zombieCanMove)
        {
            animatorController.Sprint(false);

            time += 1 * Time.deltaTime;
            if (time >= 4)
            {
                routine = Random.Range(0, 2);
                time = 0;
            }
            switch (routine)
            {
                case 0:
                    animatorController.Walk(false);
                    break;

                case 1:
                    grade = Random.Range(0, 360);
                    angle = Quaternion.Euler(0, grade, 0);
                    routine++;
                    break;

                case 2:
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, angle, zombieData.speedRotationWalk);
                    transform.Translate(Vector3.forward * zombieData.speedWalk * Time.deltaTime);
                    animatorController.Walk(true);
                    break;
            }
        }
        else
        {
            if ((Vector3.Distance(transform.position, zombieData.GetPlayer().transform.position) > 1) && zombieCanMove)
            {
                var lookPos = zombieData.GetPlayer().transform.position - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, zombieData.speedRotationRun);

                animatorController.Walk(false);
                animatorController.Sprint(true);

                transform.Translate(Vector3.forward * zombieData.speedRun * Time.deltaTime);

                animatorController.HitMelee(false);
            }
            else
            {
                if (zombieCanMove)
                { 
                    animatorController.Walk(false);
                    animatorController.Sprint(false);

                    if (Time.time > lastHitMelee + 1.8f)
                    {
                        animatorController.HitMelee(true);
                        lastHitMelee = Time.time;
                    }
                }

                else
                {
                    return;
                }

            }

        }
    }

    void SetEnabled(bool enabled)
    {
        bool isKinematic = !enabled;
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = isKinematic;
        }

        animator.enabled = !enabled;
    }

    public virtual void OnEntityDeadHandler()
    {
        this.animatorController.Dead(true);
        SetEnabled(true);
        zombieCanMove = false;
        zombieCanHit = false;
        this.gameManager.AddKill();
    }

    private void OnDestroy()
    {
        healthSystem.OnEntityDead -= OnEntityDeadHandler;
    }
}
