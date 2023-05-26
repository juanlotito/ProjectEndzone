using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPackController : MonoBehaviour
{
    private float amountHeal = 50f;
    [SerializeField] FirstPersonController player;
    [SerializeField] private HealthSystem healthSystem;
    private bool hasCollided = false;

    private void OnTriggerEnter(Collider other)
    {

        if (hasCollided) return;

        Destroy(gameObject);

        if (other.gameObject.CompareTag("Player"))
        {
            healthSystem.HealPackPicked(this.amountHeal);
        }

        hasCollided = true;

    }
}
