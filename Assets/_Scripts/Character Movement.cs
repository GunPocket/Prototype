using UnityEngine;

public class CharacterMovement : MonoBehaviour {

    [Header("Player Settings")]
    [SerializeField] private float playerSpeed = 5.0f;

    [SerializeField] private float rotationSpeed = 10.0f;
    [SerializeField] private Material playerMaterial;

    [Header("Needed References")]
    [SerializeField] private Rigidbody rb;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private InputHandler Input;
    [SerializeField] private AnimationHandler animationHandler;

    private void Start() {
        Input = Input != null ? Input : GetComponent<InputHandler>();
        mainCamera = mainCamera != null ? mainCamera : Camera.main;
        rb = rb != null ? rb : GetComponent<Rigidbody>();
        animationHandler = animationHandler != null ? animationHandler : GetComponent<AnimationHandler>();
    }

    private void FixedUpdate() {
        Vector2 direction = Input.InputDirection;
        Move(direction);

        if (animationHandler != null) {
            animationHandler.UpdateAnimation(direction);
        }
    }

    private void Move(Vector2 direction) {
        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;
        forward.y = 0;
        right.y = 0;

        Vector3 moveDirection = (forward * direction.y + right * direction.x).normalized;

        if (direction != Vector2.zero) {
            rb.velocity = moveDirection * playerSpeed;

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        } else rb.velocity = Vector3.zero;
    }
}