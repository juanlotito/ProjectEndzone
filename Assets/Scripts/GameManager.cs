using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private FirstPersonController player;
    [SerializeField] private PostProcessController postProcessController;
    [SerializeField] private GameObject lienzo;
    [SerializeField] private GameObject canvas;
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

    private void Update()
    {
        CheckWin();
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

    public void TakeDamageOnPlayer(int damage)
    {
        player.Hitted(damage);
        postProcessController.ShowDamageVignette();
    }

    public float GetPlayerHP()
    {
        return this.player.GetHp();
    }

    public int GetKills () 
    {
        return this.totalKills;
    }

    public void CheckWin()
    {
        if(totalKills >= 1)
        {
            this.player.SetPlayerCanMove(false);
            lienzo.SetActive(true);
            canvas.SetActive(true);
            GameObject[] gameObjects = FindObjectsOfType<GameObject>();
            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject.CompareTag("Player") || gameObject.CompareTag("Canvas"))
                {
                    continue;
                }

                Destroy(gameObject);
            }
        }
    }


}
