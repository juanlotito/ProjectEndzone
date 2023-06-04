using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessController : MonoBehaviour
{
    [SerializeField] private Volume postProcess;



    private void Awake()
    {
         postProcess.sharedProfile.TryGet(out Vignette vignette);
    }


    public void ShowDamageVignette()
    {
        if (postProcess.sharedProfile.TryGet(out Vignette vignette))
        {
            vignette.color.Override(Color.red);
            vignette.intensity.Override(1f);

            Invoke("ResetVignette", 0.5f);
        }
    }

    private void ResetVignette()
    {
        if (postProcess.sharedProfile.TryGet(out Vignette vignette))
        {
            vignette.color.Override(Color.black);
            vignette.intensity.Override(0.371f);
        }

    }
}   
