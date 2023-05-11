using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int totalKills;
    public int currentZombies;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        totalKills = 0;
        currentZombies = 0;
    }

    public void AddKill ()
    {
        totalKills++;
        print("Se ha sumado una kill. Total: " + totalKills);
        currentZombies--;
    }

    public void AddZombie()
    {
        this.currentZombies++;
    }


}
