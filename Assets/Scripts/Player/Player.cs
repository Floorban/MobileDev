using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private DynamicJoystick joystick;
    [SerializeField] private Canvas inputCanvas;
    public bool isJoystick;

    [Header("Movement")]
    private Rigidbody2D rb;
    public Vector2 moveDir;
    public Vector2 lastMoveDir;
    public float moveSpeed;
    public float maxSpeed;
    public float acceleration = 10f;
    public float drag = 0.5f;
    private void Awake() {
        InitComponents();
        EnableJoystick();
    }
    private void Update() {
        JoystickInput();
    }
    private void FixedUpdate() {
        Move();
    }
    private void InitComponents() {
        rb = GetComponent<Rigidbody2D>();
        rb.linearDamping = drag;
    }
    private void EnableJoystick() {
        isJoystick = true;
        inputCanvas.gameObject.SetActive(true);
    }
    private void JoystickInput() {
        if (isJoystick) {
            moveDir = new Vector2(joystick.Direction.x, joystick.Direction.y).normalized;
        }
    }
    private void Move() {
        //rb.linearVelocity = moveDir * moveSpeed;

        /*        Vector2 targetVelocity = moveDir * moveSpeed;
                Vector2 velocityChange = targetVelocity - rb.linearVelocity;
                velocityChange = Vector2.ClampMagnitude(velocityChange, acceleration * Time.fixedDeltaTime);
                rb.AddForce(velocityChange, ForceMode2D.Impulse);*/

        if (moveDir.sqrMagnitude > 0.01f) {
            Vector2 force = moveDir * acceleration;
            rb.AddForce(force, ForceMode2D.Force);
            lastMoveDir = moveDir.normalized;
        }

        if (rb.linearVelocity.magnitude > maxSpeed) {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }
    public void ApplyRecoil(Vector2 dir, float force, float stunTime = 0.2f)
    {
        rb.AddForce(-dir.normalized * force, ForceMode2D.Impulse);
        StartCoroutine(RecoilSquash());
        StartCoroutine(ShootPause(stunTime));
    }
    public IEnumerator ShootPause(float duration)
    {
        isJoystick = false;
        // set the duration depending on the current weapon (type and recoil)
        // using unscaled time here
        yield return new WaitForSecondsRealtime(duration);
        isJoystick = true;
    }
    // TO DO: use leantween or dotween to replace the effect later
    public IEnumerator RecoilSquash(float duration = 0.1f, float squashAmount = 0.8f)
    {
        Vector3 originalScale = transform.localScale;
        Vector3 squashed = new Vector3(originalScale.x * squashAmount, originalScale.y / squashAmount, originalScale.z);
        transform.localScale = squashed;

        yield return new WaitForSecondsRealtime(duration);
        transform.localScale = originalScale;
    }
}
