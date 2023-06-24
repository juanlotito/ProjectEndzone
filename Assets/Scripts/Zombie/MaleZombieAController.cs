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
    private bool zombieCanMove = true;
    private Rigidbody[] rigidbodies;
    [SerializeField] private Animator animator;
    #endregion


    private void Awake()
    {
        healthSystem.OnEntityDead += OnEntityDeadHandler;
    }

    private void Start()
    {
        rigidbodies = transform.GetComponentsInChildren<Rigidbody>();
        SetEnabled(false);
        this.gameManager = GameManager.instance;
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
                animatorController.Walk(false);
                animatorController.Sprint(true);

                Vector3 targetDirection = zombieData.GetPlayer().transform.position - transform.position;
                targetDirection.y = 0;
                Quaternion rotation = Quaternion.LookRotation(targetDirection);

                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, zombieData.speedRotationRun * 2f);

                transform.Translate(Vector3.forward * zombieData.speedRun * 1.5f * Time.deltaTime);

                if (Time.time > lastHitMelee + 1.8f)
                {
                    zombieCanMove = false;
                    animatorController.AttackAmbush(true);
                    lastHitMelee = Time.time;
                    zombieCanMove = true;
                }
            }
            else
            {
                if (zombieCanMove)
                {
                    animatorController.Walk(false);
                    animatorController.Sprint(false);
                    animatorController.AttackAmbush(false);

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

    #region OLD RAYCAST SYSTEM
    //DEV COMMENT: Se deja registro del viejo sistema de Raycast para los ataques a melee. El mismo fue modificado por un sistema mas eficiente
    //y simple de implementar.

    /*private IEnumerator HitMeleeCoroutine()
    {
        animatorController.ZombieHitMelee(true);

        RaycastHit hit;
        if (Physics.Raycast(originMelee.position, originMelee.transform.forward, out hit, meleeRange))
        {
            Debug.Log("Pegue");
        }
        animatorController.ZombieHitMelee(false);
        yield return new WaitForSeconds(reloadTime);


    }

    private void HitMelee()
    {
        StartCoroutine(HitMeleeCoroutine());
    }*/
    #endregion

    public virtual void OnEntityDeadHandler()
    {
        this.animatorController.Dead(true);
        SetEnabled(true);
        zombieCanMove = false;
        zombieCanHit = false;
        this.gameManager.AddKill();
    }

    void SetEnabled(bool enabled)
    {
        bool isKinematic = !enabled;
        foreach(Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = isKinematic;
        }

        animator.enabled = !enabled;
    }


    private void OnDestroy()
    {
        healthSystem.OnEntityDead -= OnEntityDeadHandler;
    }

}
