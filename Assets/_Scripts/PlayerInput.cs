using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour {
    private PlayerInput controller;

    public Vector2 InputDirection { get; private set; }

    private void Awake() => controller = new PlayerInput();

    private void OnEnable() {
        controller.Player.Enable();
        controller.Player.Movement.performed += OnMovePerformed;
        controller.Player.Movement.canceled += OnMoveCanceled;
    }

    private void OnDisable() {
        controller.Player.Disable();
        controller.Player.Movement.performed -= OnMovePerformed;
        controller.Player.Movement.canceled -= OnMoveCanceled;
    }

    private void OnMovePerformed(InputAction.CallbackContext context) => InputDirection = context.ReadValue<Vector2>();

    private void OnMoveCanceled(InputAction.CallbackContext context) => InputDirection = Vector2.zero;
}