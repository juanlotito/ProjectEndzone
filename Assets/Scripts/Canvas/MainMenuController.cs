using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject lienzo;
    public GameObject credits;
    public GameObject preGame;
    public EnemySpawner enemySpawner;

    private void Update()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

    }

    public void PlayButton()
    {
        //SETEO EL SPAWNEO EN ON Y ESO DA INICIO AL JUEGO
        enemySpawner.SetEnabledSpawn(true);
        preGame.SetActive(false);
        lienzo.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void MainMenuPlay()
    {
        preGame.SetActive(true);
    }

    public void CreditsButton()
    {
        credits.SetActive(true);
               
    }

    public void CreditsBackButton()
    {
        credits.SetActive(false);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    
}
