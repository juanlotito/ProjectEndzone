using System;
using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{

    [SerializeField] private float maxHealth;
    [SerializeField] private float invencibilityTime;
    private float currentHealth;
    private bool invencible;

    public event Action OnEntityDead;
    public event Action<float> OnEntityDamaged;
    public event Action<float> OnEntityHealed;

    public UnityEvent OnHealthPowerUpPicked;

    private void Awake()
    {
        OnEntityDead += Die;
        OnEntityDamaged += TakeDamage;
        OnEntityHealed += GetHeal;
    }

    public void Init()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (invencible)
            return;

        currentHealth -= damage;
        Debug.Log("Golpe! Vida restante: " + currentHealth);

        if (currentHealth <= 0)
        {
            OnEntityDead?.Invoke();
        }
    }

    public void HealPackPicked(float healAmount)
    {
        Debug.Log("El jugador agarró un HealPack!");
        OnEntityHealed?.Invoke(healAmount);
    }

    public void GetHeal(float healAmount)
    {
        currentHealth += healAmount;

        if (currentHealth > 100)
            currentHealth = 100;
    }

    private void Die()
    {
        invencible = true;
        currentHealth = 0;
    }

    public float GetCurrentHealth() => this.currentHealth;

    public void HealthPowerUpPicked ()
    {
        OnHealthPowerUpPicked?.Invoke();
    }

    public void EntityHitted(float damage)
    {
        OnEntityDamaged.Invoke(damage);
    }

    public void RaiseMaxHp(float amount)
    {
        Debug.Log("La vida del jugador subió!");
        this.maxHealth += amount;
        this.currentHealth = maxHealth;
    }

}
