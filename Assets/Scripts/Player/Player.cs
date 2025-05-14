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
    private Vector2 moveDir;
    public float moveSpeed;
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
        rb.linearVelocity = moveDir * moveSpeed;
    }
}
