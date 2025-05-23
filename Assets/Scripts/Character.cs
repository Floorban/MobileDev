using DG.Tweening;
using UnityEngine;

public class Character : MonoBehaviour
{
    public bool active;

    [Header("Anim Settings")]
    public float squashAmount = 0.8f;
    public float stretchAmount = 1.1f;
    public float squashDuration = 0.2f;
    public float holdDuration = 0.05f;
    public float returnDuration = 0.2f;

    private Vector3 originalScale;
    private Sequence squashLoop;
    public bool Active
    {
        get => active;
        set
        {
            if (active == value) return;
            active = value;

            if (value)
                StartSquashLoop();
            else
                StopSquashLoop();
        }
    }
    private void Awake()
    {
        originalScale = transform.localScale;
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
