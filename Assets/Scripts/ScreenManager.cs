using System;
using System.Diagnostics;

using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class ScreenManager : MonoBehaviour
{
    public LevelSelector LevelSelector;
    private int movesLeft;
    public TextMeshProUGUI moves;
    [SerializeField] private Image opacity;
    

    public void Setup()
    {
        gameObject.SetActive(true);
    }
 

    public void RestartButton()
    {
        gameObject.SetActive(false);
        LevelSelector.LoadLevel(LevelSelector.levelIndex);
        GameManager.Instance.InGame.gameObject.SetActive(true);
    }

    public void LvlRestart()
    {
        LevelSelector.CollectObects();
        LevelSelector.LoadLevel(LevelSelector.levelIndex);
        ScreenManager pauseobj;
        pauseobj = GameManager.Instance.Pause;
        pauseobj.transform.DOMoveY(-7f, 0.4f).OnComplete(() =>
        {
            pauseobj.gameObject.SetActive(false);
            opacity.DOFade(0f, 0.3f);
            opacity.gameObject.SetActive(false);
        }); 
    }

    public void PauseButton()
    {   

        opacity.gameObject.SetActive(true);
        opacity.DOFade(0.4f, 0.3f);
        ScreenManager pauseobj;
        pauseobj = GameManager.Instance.Pause;
        pauseobj.gameObject.SetActive(true);
        pauseobj.transform.DOMoveY(0, 0.35f).SetEase(Ease.Linear).OnComplete(() =>
        {
            pauseobj.transform.DOPunchPosition(new Vector3(0, 30, 0), 0.2f, 7, 6f);
        }); 

    }

    public void ContinueButton()
    {
        ScreenManager pauseobj;
        pauseobj = GameManager.Instance.Pause;
        pauseobj.transform.DOMoveY(-7f, 0.4f).OnComplete(() =>
        {
            pauseobj.gameObject.SetActive(false);
            opacity.DOFade(0f, 0.35f);
            opacity.gameObject.SetActive(false);
            
        }); 
        
    }

    public void MenuButton()
    {
        Debug.Log("tıklandı");
        gameObject.SetActive(false);
        LevelSelector.CollectObects();
        GameManager.Instance.InGame.gameObject.SetActive(false);
        opacity.gameObject.SetActive(false);
        GameManager.Instance.MenuScreen();
        
    }
    public void NextLevelButton()
    {
        LevelSelector.fakeLevelIndex++;
        gameObject.SetActive(false);
        if (LevelSelector.levelIndex == 6)
        {
            int randomInt = Random.Range(1, 7);
            LevelSelector.LoadLevel(randomInt);
            GameManager.Instance.InGame.gameObject.SetActive(true);
            LevelSelector.LevelSave();
        }
        else
        {
            LevelSelector.LoadLevel(LevelSelector.levelIndex + 1);
            LevelSelector.LevelSave();
            GameManager.Instance.InGame.gameObject.SetActive(true);
        }


    }
    public void StartGameButton()
    {
        gameObject.SetActive(false);
        GameManager.Instance.StartGame();
    }

    public void OnEnable()
    {
        if (gameObject.CompareTag("GameOver"))
        {
            movesLeft = GameManager.Instance.MovesLeft;
            moves.text = movesLeft.ToString() + " moves left ";
        }
        
    }
}
