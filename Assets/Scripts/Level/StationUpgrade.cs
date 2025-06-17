using UnityEngine;
using System;
public class StationUpgrade : MonoBehaviour
{
    ChefStation staion;
    [SerializeField] private float enterDuration;
    [SerializeField] private float enterTimer;
    private bool nearby;
    private bool PlayerInRange
    {
        get => nearby;
        set
        {
            nearby = value;
        }
    }

    public static event Action<ChefStation> OnShopEnter;
    public static event Action OnShopExit;
    private void Update()
    {
        if (PlayerInRange)
            enterTimer += Time.fixedDeltaTime;
        else
            enterTimer = 0;

        if (enterTimer > enterDuration)
            OnShopEnter?.Invoke(staion);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == staion.player.gameObject)
            PlayerInRange = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == staion.player.gameObject)
        {
            PlayerInRange = false;
            OnShopExit?.Invoke();
        }
    }
}
