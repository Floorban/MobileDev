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

#if UNITY_EDITOR || UNITY_STANDALONE
        Debug.Log("not on mobile");
#elif UNITY_IOS || UNITY_ANDROID
        Debug.Log("on mobile");
#endif
    }
    private void Update() {
        HandleShootInput();
        HandleReloadInput();
        HandleSwitchWeaponInput();
    }
    private void HandleShootInput() {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButtonDown(0)) {
            currentGun.OnTouchBegin(Input.mousePosition);
            StartAiming();
        }
        else if (Input.GetMouseButton(0)) {
            currentGun.OnTouchDrag(Input.mousePosition);
            UpdateAimLine(currentGun.aimDir);
        }
        else if (Input.GetMouseButtonUp(0)) {
            currentGun.OnTouchEnd();
            StopAiming();
        }
#elif UNITY_IOS || UNITY_ANDROID
        if (Input.touchCount == 1) {
            Touch touch = Input.GetTouch(0);
            if (isSwipingTwoFingers) return;
            switch (touch.phase) {
                case TouchPhase.Began:
                    currentGun.OnTouchBegin(touch.position);
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
 #endif
    }
    private void HandleReloadInput() {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetKeyDown(KeyCode.R))
            currentGun.Reload();
#elif UNITY_IOS || UNITY_ANDROID
        Vector3 acc = Input.acceleration;
        if (acc.sqrMagnitude > shakeIntensity) {
            currentGun.Reload();
        }
#endif
    }
    private Vector2 swipeStartPos;
    private bool isSwipingTwoFingers = false;
    private void HandleSwitchWeaponInput() {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.mouseScrollDelta.y != 0)
            CycleGun(Input.mouseScrollDelta.y);
#elif UNITY_IOS || UNITY_ANDROID
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
#endif
    }
    [SerializeField] private float cycleCooldown = 0.3f;
    private float lastCycleTime = -Mathf.Infinity;
    private void CycleGun(float cycleOrder) {
        if (Time.time - lastCycleTime <= cycleCooldown) return;
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
        lastCycleTime = Time.time;
    }
    private void StartAiming() {
        if (currentGun.fireMode == GunBase.FireMode.Auto || isAiming) return;
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
        aimLine.SetPosition(1, (Vector2)transform.position + currentGun.aimDir);
        //aimLine.SetPosition(1, touchWorldPos);
    }
}
