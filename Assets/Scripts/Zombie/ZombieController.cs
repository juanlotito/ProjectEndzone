using System.Collections;
using UnityEngine;

public abstract class ZombieController : MonoBehaviour
{
    private GameManager gameManager;
    public ZombieData zombieData; 
    [SerializeField] private Transform originMelee;
    public CharacterAnimatorController animatorController;
    private int currentHp;

    protected virtual void Update()
    {
        ZombieBehavor();
        CheckAlive();
    }

    private void Start()
    {
        gameManager = GameManager.instance;
        currentHp = zombieData.HP;
    }

    public abstract void ZombieBehavor();

    public virtual void EndAttack ()
    {
        animatorController.ZombieHitMelee(false);
    }

    public virtual IEnumerator HitMeleeCoroutine()
    {
        animatorController.ZombieHitMelee(true);

        RaycastHit hit;
        if (Physics.Raycast(originMelee.position, originMelee.transform.forward, out hit, zombieData.meleeRange))
        {
            Debug.Log("Pegue");
        }
        animatorController.ZombieHitMelee(false);
        yield return new WaitForSeconds(zombieData.reloadTime);

        
    }

    public virtual void HitMelee()
    {
        StartCoroutine(HitMeleeCoroutine());
        gameManager.TakeDamageOnPlayer((int)zombieData.meleDamage);
    }

    public virtual void TakeDamage(int damage)
    {
        currentHp -= damage;
    }

    public virtual void CheckAlive()
    {
        if (currentHp <= 0)
        {
            Destroy(gameObject);
            animatorController.ZombieDead(true);
            GameManager.instance.AddKill();

        }       
    }

}
