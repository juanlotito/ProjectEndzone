using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject lienzo;

    private void Update()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

    }

    public void PlayButton()
    {
        lienzo.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }


    public void ExitButton()
    {
        Application.Quit();
    }

    
}
