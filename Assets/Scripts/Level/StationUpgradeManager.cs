using UnityEngine;
using UnityEngine.UI;

public class StationUpgradeManager : MonoBehaviour
{
    private CollectionManager collection;
    public GameObject Panel;
    public ChefStation curStation;
    public WeaponStats weapon;
    public Button b_stationHP, b_stationCD, b_stationRange;
    //public Button b_weaponDMG, b_weaponRange, b_weaponKB;

    [Header("Chef Stats")]
    [SerializeField] private int maxHealth;
    [SerializeField] private float coolDown;
    [SerializeField] private float interactRange;

    [Header("Weapon Stats")]
    [SerializeField] private int damage;
    [SerializeField] private float attackRange;
    [SerializeField] private float knockBackForce;

    private void Awake()
    {
        StationInteraction.OnShopEnter += OnOpen;
        StationInteraction.OnShopExit += OnClose;
        collection = FindFirstObjectByType<CollectionManager>();
        Panel.SetActive(false);
    }
    public void OnOpen(ChefStation station)
    {
        Debug.Log("shop opened");
        Panel.SetActive(true);
        FindStation(station);

        b_stationHP.onClick.RemoveAllListeners();
        b_stationCD.onClick.RemoveAllListeners();
        b_stationRange.onClick.RemoveAllListeners();

        b_stationHP.onClick.AddListener(StationHP);
        b_stationCD.onClick.AddListener(StationCD);
        b_stationRange.onClick.AddListener(StationRange);
    }
    public void OnClose()
    {
        Panel.SetActive(false);
        ClearStation();
    }
    public void FindStation(ChefStation station) => curStation = station;
    public WeaponStats FindStation() => weapon = curStation.attack;
    public void ClearStation()
    {
        curStation = null;
        weapon = null;
    }
    public void StationHP()
    {
        if(collection.GetTips() > 1)
        {
            curStation.UpgradeHP();
            collection.ConsumeTips(1);
        }
    }
    public void StationCD() => curStation.UpgradeCD(1);
    public void StationRange() => curStation.UpgradeRange(1);
}
