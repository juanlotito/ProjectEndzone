using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandScript : MonoBehaviour
{
    private ZombieController zombieController;

    // Start is called before the first frame update
    void Start()
    {
        zombieController = GetComponentInParent<ZombieController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        zombieController.HandleCollision(other);
    }

}
