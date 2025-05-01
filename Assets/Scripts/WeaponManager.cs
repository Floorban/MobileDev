using UnityEngine;
using UnityEngine.InputSystem;
using TouchPhase = UnityEngine.TouchPhase;

public class WeaponManager : MonoBehaviour
{
    public Vector3 angularVelocity;
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
            switch (touch.phase) {
                case TouchPhase.Began:
                    StartAiming();
                    currentGun.OnTouchBegin(touch.position);
                    break;
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    currentGun.OnTouchDrag(touch.position);
                    Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    UpdateAimLine(worldPos);
                    break;
                case TouchPhase.Ended:
                    //currentGun.OnTouchEnd(touch.position);
                    Vector2 worldPoss = Camera.main.ScreenToWorldPoint(touch.position);
                    Vector2 direction = (worldPoss - (Vector2)transform.position).normalized;
                    Fire(direction);
                    StopAiming();
                    break;
            }
        }
//#endif
    }
    private void HandleReloadInput() {
        Vector3 acc = Input.acceleration;
        if (acc.sqrMagnitude > 10f) {
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

                if (Mathf.Abs(swipeDelta.y) > 100f && Mathf.Abs(swipeDelta.y) > Mathf.Abs(swipeDelta.x)) {
                    if (swipeDelta.y > 0) {
                        Debug.Log("Two-finger swipe up - Cycle weapon up");
                        CycleGun();
                    }
                    else {
                        Debug.Log("Two-finger swipe down - Cycle weapon down");
                        CycleGun(); 
                    }
                }

                isSwipingTwoFingers = false;
            }
        }
        else {
            isSwipingTwoFingers = false;  
        }
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
    private void Fire(Vector2 direction) {
        if (!currentGun || !currentGun.TryFire(currentGun.AmmoCostPerShot)) return;

        currentGun.ShootProjectile(currentGun.inputAimDIr * direction);
        //currentGun.OnTouchEnd(direction);

        if (player) {
            player.ApplyRecoil(currentGun.inputAimDIr * direction, currentGun.recoilForce);
            StartCoroutine(player.RecoilSquash());
            StartCoroutine(player.ShootPause());
        }
    }
}
