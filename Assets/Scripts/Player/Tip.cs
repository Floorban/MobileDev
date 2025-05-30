using UnityEngine;
using System;

public class Tip : Pickup, ICollectible
{
    public int tipAmount;
    public static event Action<int> OnTipCollected;
    public void Collect()
    {
        OnTipCollected?.Invoke(tipAmount);
        Debug.Log("tip collected");
        Destroy(gameObject);
    }
}
