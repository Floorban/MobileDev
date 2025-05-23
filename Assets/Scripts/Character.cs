using DG.Tweening;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Squash Settings")]
    public float squashAmount = 0.8f;
    public float stretchAmount = 1.1f;
    public float squashDuration = 0.2f;
    public float holdDuration = 0.05f;
    public float returnDuration = 0.2f;

    private Vector3 originalScale;
    private Sequence squashLoop;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    private void Start()
    {
        StartSquashLoop();
    }

    private void StartSquashLoop()
    {
        StopSquashLoop();

        squashLoop = DOTween.Sequence();
        squashLoop.Append(transform.DOScale(
                new Vector3(originalScale.x * stretchAmount, originalScale.y * squashAmount, originalScale.z),
                squashDuration
            ).SetEase(Ease.OutQuad))
            .Append(transform.DOScale(originalScale, squashDuration).SetEase(Ease.InOutQuad))
            .AppendInterval(holdDuration)
            .SetLoops(-1);
            //.SetUpdate(true);
    }
    public void StopSquashLoop()
    {
        if (squashLoop != null && squashLoop.IsActive())
        {
            squashLoop.Kill();
            squashLoop = null;
            transform.DOScale(originalScale, returnDuration).SetEase(Ease.OutElastic);
        }
    }
}
