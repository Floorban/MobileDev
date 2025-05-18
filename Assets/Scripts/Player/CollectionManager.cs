using UnityEngine;

public class CollectionManager : MonoBehaviour
{
    [SerializeField] private int numOfTip = 0;
    private void OnEnable()
    {
        Tip.OnTipCollected += TipCollected;
    }
    private void OnDisable()
    {
        Tip.OnTipCollected -= TipCollected;
    }
    private void TipCollected(int increasedAmount)
    {
        numOfTip += increasedAmount;
    }
}
