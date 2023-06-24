using System.Collections;
using UnityEngine;

public abstract class ZombieController : MonoBehaviour
{
    public GameManager gameManager;
    public ZombieData zombieData; 
    [SerializeField] private Transform originMelee;
    public CharacterAnimatorController animatorController;
    public HealthSystem healthSystem;
    protected bool zombieCanHit = true;

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

    public void HandleCollision(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && zombieCanHit)
        {
            Vector3 knockbackDirection = other.gameObject.transform.position - this.transform.position;
            knockbackDirection.Normalize();
            other.attachedRigidbody.AddForce(knockbackDirection * this.zombieData.knockbackForce, ForceMode.Impulse);
            gameManager.TakeDamageOnPlayer((int)zombieData.meleDamage);
        }
    }
 
    public virtual void TakeDamage(int damage)
    {
        this.healthSystem.TakeDamage(damage);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(originMelee.position, originMelee.transform.forward * zombieData.meleeRange);
    }
}
