using UnityEngine;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera menucam;

    private Vector3 startPos;
    private Quaternion startRot;
    void Start()
    {
        startPos = menucam.transform.position;
        startRot = menucam.transform.rotation;

        Sequence sequence = DOTween.Sequence();

        sequence.AppendCallback(() =>
        {
            menucam.transform.position = startPos;
            menucam.transform.rotation = startRot;
        });

        // 1. Z ekseninde ileri git
        sequence.Append(menucam.transform.DOMoveZ(startPos.z + 5, 10f));

        // 2. Başka bir pozisyona ışınla
        sequence.AppendCallback(() =>
        {
            menucam.transform.position = new Vector3(1225, -437, -184);
            menucam.transform.rotation = Quaternion.Euler(0, -39, 0);
        });

        // 3. X ekseninde kay
        sequence.Append(menucam.transform.DOMoveX(menucam.transform.position.x + 13, 10f).SetEase(Ease.Linear));

        // Sonsuza kadar tekrar et
        sequence.SetLoops(-1, LoopType.Restart);
    }
}