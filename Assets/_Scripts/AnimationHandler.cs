using UnityEngine;

public class AnimationHandler : MonoBehaviour {
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody playerRigidbody;

    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Punching = Animator.StringToHash("Punch");

    public void UpdateAnimation(Vector2 direction) {
        bool isIdle = direction == Vector2.zero;
        animator.SetBool(Idle, isIdle);
    }

    public void TriggerPunch() => animator.SetTrigger(Punching);
}