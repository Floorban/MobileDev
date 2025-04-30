using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [Header("Movement")]
    private Rigidbody2D rb;
    public bool canMove = false;
    [SerializeField] private float moveSpeed = 3f;
    private int moveDir = 1; // 1 for right, -1 for left

    [Header("Shooting")]
    public GameObject bulletPrefab;
    [SerializeField] private Transform shootPoint;
    private LineRenderer aimLine;
    private Vector2 aimDir;
    private bool isAiming = false;

    [Header("Recoil")]
    [SerializeField] private float recoilForce = 10f;
    [SerializeField] private float movePauseDuration = 0.2f;
    [SerializeField] private float slowMotionScale = 0.2f;
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        aimLine = GetComponent<LineRenderer>();
        if (aimLine) aimLine.enabled = false;
    }
    private void Update() {
        TouchInput();
    }
    private void FixedUpdate() {
        HorizonMove();
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.CompareTag("Wall")) {
            foreach (ContactPoint2D contact in collision.contacts) {
                // hit from the side
                if (Mathf.Abs(contact.normal.x) > 0.5f) {
                    Flip();
                    break;
                }
            }
        }
    }
    private void HorizonMove() {
        if (!canMove) return;
        rb.linearVelocity = new Vector2(moveDir * moveSpeed, rb.linearVelocity.y);
    }
    private void Flip() {
        moveDir *= -1;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    private void TouchInput() {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
        if (Input.GetMouseButtonDown(0)) {
            StartAiming();
        }
        else if (Input.GetMouseButton(0)) {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            aimDir = (mouseWorldPos - (Vector2)transform.position).normalized;
            UpdateAimLine(mouseWorldPos);
        }
        else if (Input.GetMouseButtonUp(0)) {
            Shoot(aimDir);
            StopAiming();
        }
#elif UNITY_IOS || UNITY_ANDROID
    if (Input.touchCount > 0) {
        Touch touch = Input.GetTouch(0);
        Vector2 touchWorldPos = Camera.main.ScreenToWorldPoint(touch.position);
        aimDir = (touchWorldPos - (Vector2)transform.position).normalized;

        if (touch.phase == TouchPhase.Began) {
            StartAiming();
        }
        else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) {
            UpdateAimLine(touchWorldPos);
        }
        else if (touch.phase == TouchPhase.Ended) {
            Shoot(aimDir);
            StopAiming();
        }
    }
#endif
    }
    void StartAiming() {
        if (isAiming) return;
        isAiming = true;
        Time.timeScale = slowMotionScale;
        // TO DO: using private multipliers instead of timescale for all the dynamic objects slater
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        aimLine.enabled = true;
    }

    void StopAiming() {
        isAiming = false;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        aimLine.enabled = false;
    }

    void Shoot(Vector2 direction) {
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        // TO DO: determine projectile's speed from the current weapon's script later
        bullet.GetComponent<Rigidbody2D>().linearVelocity = direction * 20f; 
        Destroy(bullet, 1.5f);

        StartCoroutine(RecoilSquash());
        rb.AddForce(-direction * recoilForce, ForceMode2D.Impulse);
        StartCoroutine(ShootPause());
    }
    private IEnumerator ShootPause() {
        canMove = false;
        // set the duration depending on the current weapon (type and recoil)
        // using unscaled time here
        yield return new WaitForSecondsRealtime(movePauseDuration);
        if (!isAiming) canMove = true; 
    }
    // TO DO: use leantween or dotween to replace the effect later
    private IEnumerator RecoilSquash(float duration = 0.1f, float squashAmount = 0.8f) {
        Vector3 originalScale = transform.localScale;
        Vector3 squashed = new Vector3(originalScale.x * squashAmount, originalScale.y / squashAmount, originalScale.z);
        transform.localScale = squashed;

        yield return new WaitForSecondsRealtime(duration);
        transform.localScale = originalScale;
    }
    // TO DO: set the line as consistent distance from a ui script later
    void UpdateAimLine(Vector2 touchWorldPos) {
        if (!aimLine) return;
        aimLine.SetPosition(0, transform.position);
        //aimLine.SetPosition(1, (Vector2)transform.position + aimDir * 5f);
        aimLine.SetPosition(1, touchWorldPos);
    }
}
