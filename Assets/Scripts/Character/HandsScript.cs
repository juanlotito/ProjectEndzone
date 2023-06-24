using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsScript : MonoBehaviour
{
    private FirstPersonController playerController;

    void Start()
    {
        playerController = GetComponentInParent<FirstPersonController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        playerController.HandleCollision(other);
    }
}
