using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GunController[] allGuns;
    private int currentIndex = 0;
    public GunController currentGun;
    private Rigidbody2D playerRb;

    private void Start() {
        playerRb = GetComponent<Rigidbody2D>();
        currentGun.Initialize(playerRb);
    }

    private void Update() {
        HandleInput();
        if (Input.GetKeyDown(KeyCode.Tab)) {
            CycleGun();
        }
    }

    private void HandleInput() {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButtonDown(0))
            currentGun.OnTouchBegin(Input.mousePosition);
        else if (Input.GetMouseButton(0))
            currentGun.OnTouchDrag(Input.mousePosition);
        else if (Input.GetMouseButtonUp(0))
            currentGun.OnTouchEnd(Input.mousePosition);
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

    void CycleGun() {
        currentIndex = (currentIndex + 1) % allGuns.Length;
        currentGun = allGuns[currentIndex];
        currentGun.Initialize(playerRb);
    }
}
