using TMPro;
using UnityEngine;

public class CollectionManager : MonoBehaviour
{
    [SerializeField] private int numOfTip = 0;
    [SerializeField] private TextMeshProUGUI amountText;
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
        amountText.text = numOfTip.ToString();
    }
}
