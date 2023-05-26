using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicPowerUpController : MonoBehaviour
{
    private bool hasCollided = false;
    private FirstPersonController player;

    public void OnTriggerEnter(Collider other)
    {
        if (hasCollided) return;

        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            player = other.GetComponent<FirstPersonController>();
            MakeEffect(player);
        }

        hasCollided = true;
    }

    public abstract void MakeEffect(FirstPersonController player);
}
