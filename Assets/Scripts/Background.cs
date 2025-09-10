using UnityEngine;
using TMPro;
public class BackGround : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moves;
    public int movesLeft;
    
    void Update()
    {
        movesLeft = GameManager.Instance.MovesLeft;
        moves.text = movesLeft.ToString() + " moves left ";
    }
}
