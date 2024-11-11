using System.Collections;
using UnityEngine;

public class CollisionHandler : MonoBehaviour {
    [SerializeField] private float forceMagnitude = 100f;
    [SerializeField] private StackHandler stackHandler;
    [SerializeField] private AnimationHandler animationHandler;

    private void Start() {
        if (stackHandler == null) stackHandler = FindFirstObjectByType<StackHandler>();
        if (animationHandler == null) animationHandler = FindFirstObjectByType<AnimationHandler>();
    }

    private void OnCollisionEnter(Collision collision) {
        Rigidbody rb = collision.rigidbody;
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.CompareTag("Punchable") && rb != null) {
            DisableAnimatorAndRootCollider(collidedObject);
            ApplyForceToRagdoll(collidedObject, -collision.contacts[0].normal);
            ChangeTag(collidedObject, "Pickable");
            animationHandler.TriggerPunch();
        }

        if (collidedObject.transform.root.CompareTag("Pickable") && stackHandler != null) {
            if (!stackHandler.CanCarry()) return;

            PrepareObjectForStack(collidedObject.transform.root.gameObject);

            Animator animator = collidedObject.transform.root.GetComponentInChildren<Animator>();
            if (animator != null) animator.enabled = true;

            stackHandler.AddCarriedObject(collidedObject.transform.root.GetComponent<Rigidbody>());
        }
    }

    private void DisableAnimatorAndRootCollider(GameObject obj) {
        Animator animator = obj.GetComponentInChildren<Animator>();
        if (animator != null) animator.enabled = false;

        Collider rootCollider = obj.GetComponent<Collider>();
        if (rootCollider != null && rootCollider.transform == obj.transform) rootCollider.enabled = false;
    }

    private void ApplyForceToRagdoll(GameObject obj, Vector3 forceDirection) {
        Rigidbody[] childRigidbodies = obj.GetComponentsInChildren<Rigidbody>();
        for (int i = 0; i < childRigidbodies.Length; i++) {
            Rigidbody childRb = childRigidbodies[i];
            if (childRb != null) {
                childRb.isKinematic = false;
                StartCoroutine(ApplyForceWithDelay(childRb, forceDirection));
            }
        }
    }

    private IEnumerator ApplyForceWithDelay(Rigidbody rb, Vector3 forceDirection) {
        yield return new WaitForFixedUpdate();
        rb.AddForce(forceDirection * forceMagnitude, ForceMode.VelocityChange);
    }

    private void ChangeTag(GameObject gameObject, string tag, float time = 1f) => StartCoroutine(ChangeTagWithDelay(gameObject, tag, time));

    private IEnumerator ChangeTagWithDelay(GameObject obj, string tag, float time) {
        yield return new WaitForSeconds(time);
        obj.tag = tag;
    }

    private void PrepareObjectForStack(GameObject obj) {
        obj.tag = "OnStack";
        Rigidbody[] childRigidbodies = obj.GetComponentsInChildren<Rigidbody>();
        for (int i = 0; i < childRigidbodies.Length; i++) {
            Rigidbody childRb = childRigidbodies[i];
            if (childRb != null) childRb.useGravity = false;
        }

        Collider rootCollider = obj.GetComponent<Collider>();
        if (rootCollider != null && rootCollider.transform == obj.transform) rootCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Sell") && stackHandler != null && stackHandler.HasCarriedObjects()) {
            Observer.Instance.Money += stackHandler.GetCarriedObjectCount() * 100;
            stackHandler.RemoveAllCarriedObjects();
        }
    }
}