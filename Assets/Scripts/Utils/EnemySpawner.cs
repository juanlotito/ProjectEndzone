using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnPoints; 
    [SerializeField] private GameObject zombiePrefab;
    private int maxZombies = 5;
    


    private void Update()
    {
        SpawnZombies();    
    }

    private void SpawnZombies()
    {
        var currentZombies = GameManager.instance.currentZombies;

        if (currentZombies >= maxZombies)
        {
            return;
        }

        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPointChosen = spawnPoints[spawnIndex].transform;

        Instantiate(zombiePrefab, spawnPointChosen.position, spawnPointChosen.rotation);
        GameManager.instance.AddZombie();
    }

}
