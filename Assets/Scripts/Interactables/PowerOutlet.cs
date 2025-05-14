using TMPro;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PowerOutlet : MonoBehaviour
{
    [SerializeField] Ability power;
    private GameObject player;
    public float rangeRadius = 5f;
    private bool connected;
    private bool nearby;

    private RopeComponent rc;
    //[SerializeField] CameraController cameraController;

    [Header("UI")]
    [SerializeField] private GameObject uiPanel;
    private TextMeshProUGUI uiPrompt;
    [SerializeField] string activatePrompt;
    [SerializeField] string deactivatePrompt;

    public bool Connected
    {
        get => connected;
        set
        {
            connected = value;
            //WeaponFire(value);
            HandleWire(value);
            //HandleCam(value);
            //CheckUI(value);
            power.Activation(value);
        }
    }

    private bool Nearby
    {
        get => nearby;
        set
        {
            nearby = value;
            //CheckUI(value);
        }
    }

    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
        rc = GetComponent<RopeComponent>();
        player = FindAnyObjectByType<PlayerAddon>().gameObject;
        if (power) power.performPoint = player.transform;
        if (uiPanel != null)
        {
            uiPrompt = uiPanel.GetComponentInChildren<TextMeshProUGUI>();
            uiPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (Nearby && !Connected)
            Connected = true;
        HandleLocation();
    }

    private void HandleLocation()
    {
        if (!CheckRange())
        {
            // only update if it's currently true so it doesn't set connect to false every frame
            if (Connected) 
                Connected = false;
        }
        else if (Connected)
            rc.ropeLength = Mathf.Clamp(Vector3.Distance(transform.position, player.transform.position) - 1f, 0.5f, Vector3.Distance(transform.position, player.transform.position) - 1f);
    }

/*    private void WeaponFire(bool enable)
    {
        var combatManager = player.GetComponent<CombatManager>();
        combatManager.enabled = enable;
    }*/

    private bool CheckRange()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        return distance <= rangeRadius;
    }
    private void HandleWire(bool hasConnected)
    {
        if (hasConnected)
        {
            rc.InitRopePoints(transform, player.transform);
            rc.InitLineRenderer();
            rc.Connected = true;
        }
        else
        {
            rc.DisableRope();
        }
    }
/*    private void HandleCam(bool enter)
    {
        if (enter)
            StartCoroutine(cameraController.LevelOverview());
        else
            cameraController.LevelExit(false);
    }*/
    private void CheckUI(bool active)
    {
        if (connected)
            uiPrompt.text = deactivatePrompt;
        else
            uiPrompt.text = activatePrompt;

        if (active)
            uiPanel.SetActive(true);
        else
            uiPanel.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject == player)
            Nearby = true;
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject == player)
            Nearby = false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangeRadius);
    }
}
