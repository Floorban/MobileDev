using UnityEngine;
using UnityEngine.InputSystem;
using TouchPhase = UnityEngine.TouchPhase;

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

    [Header("InputIntensity")]
    public float shakeIntensity = 10f;
    public float cycleDistance = 100f;
    private void Awake() {
        player = GetComponentInParent<PlayerController>();
        aimLine = GetComponent<LineRenderer>();
        if (aimLine) aimLine.enabled = false;
        if (allGuns.Length > 0) currentGun = allGuns[0];
        if (currentGun) {
            currentGun.Initialize();
            currentGun.Setup(player);
        }
    }
    private void Update() {
        HandleShootInput();
        HandleReloadInput();
        HandleSwitchWeaponInput();
    }
    private void HandleShootInput() {
/*#if UNITY_EDITOR || UNITY_STANDALONE
*//*        if (Input.GetMouseButtonDown(0))
            currentGun.OnTouchBegin(Input.mousePosition);
        else if (Input.GetMouseButton(0))
            currentGun.OnTouchDrag(Input.mousePosition);
        else if (Input.GetMouseButtonUp(0))
            currentGun.OnTouchEnd(Input.mousePosition);*//*

        if (Input.GetMouseButtonDown(0)) {
            StartAiming();
        }
        else if (Input.GetMouseButton(0)) {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            UpdateAimLine(worldPos);
        }
        else if (Input.GetMouseButtonUp(0)) {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (worldPos - (Vector2)transform.position).normalized;
            Fire(direction);
            StopAiming();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
            CycleGun();
#elif UNITY_IOS || UNITY_ANDROID*/
        if (Input.touchCount == 1) {
            Touch touch = Input.GetTouch(0);
            if (isSwipingTwoFingers) return;
            switch (touch.phase) {
                case TouchPhase.Began:
                    currentGun.OnTouchBegin(touch.position);
                    if (currentGun.fireMode != GunBase.FireMode.Auto)
                        StartAiming();
                    break;
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    currentGun.OnTouchDrag(touch.position);
                    UpdateAimLine(currentGun.aimDir);
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    currentGun.OnTouchEnd();
                    StopAiming();
                    break;
            }
        }
        else {
            StopAiming();
        }
        //#endif
    }
    private void HandleReloadInput() {
        Vector3 acc = Input.acceleration;
        if (acc.sqrMagnitude > shakeIntensity) {
            currentGun.Reload();
        }
    }
    private Vector2 swipeStartPos;
    private bool isSwipingTwoFingers = false;
    private void HandleSwitchWeaponInput() {
        if (Input.touchCount == 2) {
            Touch t1 = Input.GetTouch(0);
            Touch t2 = Input.GetTouch(1);

            if (t1.phase == TouchPhase.Began || t2.phase == TouchPhase.Began) {
                swipeStartPos = (t1.position + t2.position) / 2f;
                isSwipingTwoFingers = true;
            }
            else if ((t1.phase == TouchPhase.Ended || t1.phase == TouchPhase.Canceled) &&
                          (t2.phase == TouchPhase.Ended || t2.phase == TouchPhase.Canceled) &&
                          isSwipingTwoFingers) {
                Vector2 swipeEndPos = (t1.position + t2.position) / 2f;
                Vector2 swipeDelta = swipeEndPos - swipeStartPos;

                if (Mathf.Abs(swipeDelta.y) > cycleDistance) {
                        CycleGun(swipeDelta.y);
                }

                isSwipingTwoFingers = false;
            }
        }
        else {
            isSwipingTwoFingers = false;  
        }
    }
    private void CycleGun(float cycleOrder) {
        if (cycleOrder < 0) { // cycle down
            currentIndex = (currentIndex + 1) % allGuns.Length; 
        }
        else { // cycle up
            if (currentIndex > 0)
                currentIndex = (currentIndex - 1) % allGuns.Length;
            else
                currentIndex = allGuns.Length - 1;
        }

        currentGun = allGuns[currentIndex];
        if (currentGun)
            currentGun.Setup(player);
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
}
