using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private PlayerController player;

    public GunController[] allGuns;
    public GunController currentGun;
    private int currentIndex = 0;

    [Header("Aiming")]
    private LineRenderer aimLine;
    private bool isAiming = false;
    [SerializeField] private float slowMotionScale = 0.3f;
    private void Awake() {
        player = GetComponentInParent<PlayerController>();
        aimLine = GetComponent<LineRenderer>();
        if (aimLine) aimLine.enabled = false;
        if (allGuns.Length > 0) currentGun = allGuns[0];
        if (currentGun) currentGun.Initialize();
    }
    private void Update() {
        HandleInput();
        currentGun.HandleReloadInput();
    }
    private void HandleInput() {
#if UNITY_EDITOR || UNITY_STANDALONE
/*        if (Input.GetMouseButtonDown(0))
            currentGun.OnTouchBegin(Input.mousePosition);
        else if (Input.GetMouseButton(0))
            currentGun.OnTouchDrag(Input.mousePosition);
        else if (Input.GetMouseButtonUp(0))
            currentGun.OnTouchEnd(Input.mousePosition);*/

        if (Input.GetMouseButtonDown(0)) {
            StartAiming();
            currentGun.OnTouchBegin(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0)) {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            UpdateAimLine(worldPos);
            currentGun.OnTouchDrag(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0)) {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (worldPos - (Vector2)transform.position).normalized;
            TriggerFire(direction);
            StopAiming();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
            CycleGun();
#elif UNITY_IOS || UNITY_ANDROID
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase) {
                case TouchPhase.Began:
                    currentGun.OnTouchBegin(touch.position);
                    break;
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    currentGun.OnTouchDrag(touch.position);
                    break;
                case TouchPhase.Ended:
                    currentGun.OnTouchEnd(touch.position);
                    break;
            }
        }
#endif
    }
    private void CycleGun() {
        currentIndex = (currentIndex + 1) % allGuns.Length;
        currentGun = allGuns[currentIndex];
        if (currentGun) currentGun.Setup();
    }
    private Vector2 GetAimDir(Vector2 screenPos) {
        Vector2 worldPos = Camera.main.ScreenToViewportPoint(screenPos);
        return (worldPos - (Vector2)transform.position).normalized;
    }
    private void StartAiming() {
        if (isAiming) return;
        isAiming = true;
        // TO DO: using private multipliers instead of timescale for all the dynamic objects slater
        Time.timeScale = slowMotionScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        if (aimLine) aimLine.enabled = true;
    }
    private void StopAiming() {
        isAiming = false;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        if(aimLine) aimLine.enabled = false;
    }
    private void UpdateAimLine(Vector2 touchWorldPos) {
        if (!aimLine) return;
        aimLine.SetPosition(0, currentGun.shootPoint.position);
        //aimLine.SetPosition(1, (Vector2)transform.position + aimDir * 5f);
        aimLine.SetPosition(1, touchWorldPos);
    }
    private void TriggerFire(Vector2 direction) {
        if (!currentGun) return;

        if (player && currentGun.TryFire(0)) {
            player.ApplyRecoil(currentGun.inputAimDIr * direction, currentGun.recoilForce);
            StartCoroutine(player.RecoilSquash());
            StartCoroutine(player.ShootPause());
        }
        //currentGun.Fire(direction);
        currentGun.OnTouchEnd(direction);
    }
}
