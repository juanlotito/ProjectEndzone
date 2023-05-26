using System.Collections;
using UnityEngine;

public abstract class ZombieController : MonoBehaviour
{
    public GameManager gameManager;
    public ZombieData zombieData; 
    [SerializeField] private Transform originMelee;
    public CharacterAnimatorController animatorController;
    public HealthSystem healthSystem;

    protected virtual void Update()
    {
        ZombieBehavor();
    }

    private void Start()
    {
        gameManager = GameManager.instance;
    }

    public abstract void ZombieBehavor();

    public virtual void EndAttack ()
    {
        animatorController.HitMelee(false);
    }

    public virtual IEnumerator HitMeleeCoroutine()
    {
        animatorController.HitMelee(true);

        RaycastHit hit;
        if (Physics.Raycast(originMelee.position, originMelee.transform.forward, out hit, zombieData.meleeRange))
        {
            Debug.Log("Pegue");
        }
        animatorController.HitMelee(false);
        yield return new WaitForSeconds(zombieData.reloadTime);

        
    }

    public virtual void HitMelee()
    {
        StartCoroutine(HitMeleeCoroutine());
        gameManager.TakeDamageOnPlayer((int)zombieData.meleDamage);
    }

    public virtual void TakeDamage(int damage)
    {
        this.healthSystem.TakeDamage(damage);
    }

}
