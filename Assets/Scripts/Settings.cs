using System;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public static Settings Instance { get; private set; }
    
    [SerializeField] private GameObject haptic;
    [SerializeField] private GameObject mute;
    bool isHapticActive;
    
    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    
    private void Start()
    {
        isHapticActive = PlayerPrefs.GetInt("Haptic", 1) == 1; 
        haptic.SetActive(!isHapticActive);
    }


    public void HapticButton()
    {
        isHapticActive = !isHapticActive;
        haptic.SetActive(isHapticActive);

        Debug.Log(!isHapticActive);
        
        PlayerPrefs.SetInt("HapticEnabled", !isHapticActive ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void Haptic()
    {
        if (isHapticActive)
        {
            Handheld.Vibrate();
        }
    }
    
}



