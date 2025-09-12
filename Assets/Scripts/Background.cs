using System;
using DG.Tweening;
using UnityEngine;
using TMPro;

public class BackGround : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moves;
    private int movesLeft;
    private int lastMovesLeft = -1;
    
    void Update()
    {
        movesLeft = GameManager.Instance.MovesLeft;
        moves.text = movesLeft.ToString() + " moves left ";
        
        if (movesLeft != lastMovesLeft)
        {
            UpdateMovesDisplay();
            lastMovesLeft = movesLeft;
        }
    }

    void UpdateMovesDisplay()
    {
        moves.transform.DOKill();
        moves.transform.localScale = Vector3.one;
        
        if (movesLeft < 4 && movesLeft >= 0)
        {
            moves.color = Color.red;
            moves.transform.DOPunchScale(Vector3.one * 0.1f, 0.3f, 10, 1f);
        }
        else
        {
            moves.color = Color.white;
        }
    }
}