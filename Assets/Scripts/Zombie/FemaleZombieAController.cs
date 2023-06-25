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
    private bool isWalking = false;
    private bool isIdle = false;
    private bool isPlayingSound = false;
    private bool hasPlayedSound = false;
    private bool isAttacking = false;
    private bool canPlay = true;
    #endregion

    #region Sounds
    private AudioSource audioSource;
    [SerializeField] private AudioClip zombieIdleSound;
    [SerializeField] private AudioClip zombieHitSound;
    [SerializeField] private AudioClip zombieWalkSound;
    [SerializeField] private AudioClip zombieDeadSound;
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
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if (isIdle)
        {
            if (!isPlayingSound && !hasPlayedSound && canPlay)
            {
                isPlayingSound = true;
                audioSource.PlayOneShot(zombieIdleSound);
                hasPlayedSound = true;
            }
            isWalking = false;
            isAttacking = false;
        }
        else if (isWalking)
        {
            if (!isPlayingSound && !hasPlayedSound && canPlay)
            {
                isPlayingSound = true;
                audioSource.PlayOneShot(zombieWalkSound);
                hasPlayedSound = true;
            }
            isIdle = false;
            isAttacking = false;
        }
        else if (isAttacking)
        {
            if (!isPlayingSound && !hasPlayedSound && canPlay)
            {
                isPlayingSound = true;
                audioSource.PlayOneShot(zombieHitSound);
                hasPlayedSound = true;
            }
            isIdle = false;
            isWalking = false;
        }
        else
        {
            isIdle = false;
            isWalking = false;
            isAttacking = false;
            isPlayingSound = false;
            hasPlayedSound = false;
            audioSource.Stop();
        }

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
                    isWalking = false;
                    isIdle = true;
                    break;

                case 1:
                    isIdle = true;
                    grade = Random.Range(0, 360);
                    angle = Quaternion.Euler(0, grade, 0);
                    routine++;
                    break;

                case 2:
                    isWalking = true;
                    isIdle = false;
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

                isIdle = false;
                isWalking = true;

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
                        zombieCanMove = false;
                        isWalking = false;
                        isIdle = false;
                        animatorController.HitMelee(true);
                        lastHitMelee = Time.time;
                        zombieCanMove = true;
                        isPlayingSound = false;
                        isHittingMelee = true;
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

    private void OnDestroy()
    {
        healthSystem.OnEntityDead -= OnEntityDeadHandler;
    }
}
