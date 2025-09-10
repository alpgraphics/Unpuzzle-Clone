using System;
using UnityEngine;

public class Settings : MonoBehaviour
{
    [SerializeField] private GameObject haptic;
    [SerializeField] private GameObject mute;
    bool isActive;

    private void Start()
    {
        isActive = false;
    }

    public void HapticButton()
    {
        if (isActive)
        {
            haptic.SetActive(false);
            isActive = false;
        }
        else
        {   
            haptic.SetActive(true);
            isActive = true;
        }
    }


}
