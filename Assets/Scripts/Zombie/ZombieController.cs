using System.Collections;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private Transform originMelee;
    [SerializeField] private CharacterAnimatorController animatorController;

    #region Behavour Variables
    private int routine;
    private float time;
    private Quaternion angle;
    private float grade;
    #endregion

    #region Patroll Variables
    private float speedWalk = 1f;
    private float speedRotationWalk = 0.5f;
    #endregion

    #region Chasing Variables
    private float speedRotationRun = 1f;
    private float speedRun = 2f;
    private float distanceToChase = 15f;
    #endregion

    #region Melee Variables
    private float meleeRange = 0.7f;
    private float lastHitMelee = 0f;
    private float reloadTime = 1f;
    #endregion

    #region Other
    private int HP = 100;
    #endregion

    private void Update()
    {
        ZombieBehavor();
        CheckAlive();
    }

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }

    void ZombieBehavor()
    {
        if (Vector3.Distance(transform.position, player.transform.position) > distanceToChase)
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
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, angle, speedRotationWalk);
                    transform.Translate(Vector3.forward * speedWalk * Time.deltaTime);
                    animatorController.ZombieWalk(true);
                    break;
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, player.transform.position)> 1)
            {
                var lookPos = player.transform.position - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, speedRotationRun);

                animatorController.ZombieWalk(false);
                animatorController.ZombieChasing(true);

                transform.Translate(Vector3.forward * speedRun * Time.deltaTime);

                animatorController.ZombieHitMelee(false);
            }
            else
            {
                animatorController.ZombieWalk(false);
                animatorController.ZombieChasing(false);
                animatorController.ZombieHitMelee(true);
                
                if (Time.time > lastHitMelee + 1f)
                {
                    HitMelee();
                    lastHitMelee = Time.time;
                }
            }
            
        }
    }

    public void EndAttack ()
    {
        animatorController.ZombieHitMelee(false);
    }


    private IEnumerator HitMeleeCoroutine()
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
    }

    public void TakeDamage(int damage)
    {
        HP -= damage;
    }

    private void CheckAlive()
    {
        if (HP<=0)
        {
            /*grade = 0f;
            distanceToChase = 10000f;
            speedWalk = 0f;
            speedRun = 0f;
            speedRotationWalk = 0f;
            speedRotationRun = 0f;*/
            Destroy(gameObject);
            animatorController.ZombieDead(true);
            GameManager.instance.AddKill();

        }       
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(view.position, transform.forward * detectionRange);
    }*/
}
