using UnityEngine;

public class StationManager : MonoBehaviour
{
    [Header("station")]
    public ChefStation curStation;


    [Header("chef stats")]
    [SerializeField] private int maxHealth;
    [SerializeField] private float coolDown;
    [SerializeField] private float interactRange;

    [Header("weapon stats")]
    [SerializeField] private int damage;
    [SerializeField] private float attackRange;
    [SerializeField] private float knockBackForce;

    private void OnEnable()
    {
        StationUpgrade.OnShopEnter += FindStation;
        StationUpgrade.OnShopExit += ClearStation;
    }
    public void FindStation(ChefStation station) => curStation = station;
    public void ClearStation() => curStation = null;

}
