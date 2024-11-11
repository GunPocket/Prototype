using System.Collections.Generic;
using UnityEngine;

public class StackHandler : MonoBehaviour {

    [Header("Settings")]
    [SerializeField] private int maxParticles = 5;

    [SerializeField] private Vector3 baseLinkRotation = Vector3.zero;
    [SerializeField] private float anchorOffsetDistance = 1.0f;
    [SerializeField] private float segmentDistance = 1.0f;
    [SerializeField] private float rotationDamp = 0.2f;

    private List<Vector3> particles = new();
    private List<GameObject> links = new();

    private void Start() {
        InitializeChain(maxParticles);
    }

    private void FixedUpdate() {
        UpdateParticles();
        UpdateLinks();
    }

    private void InitializeChain(int count) {
        ClearChain();
        Vector3 startPosition = transform.position + anchorOffsetDistance * Vector3.up;

        for (int i = 0; i < count; i++) {
            particles.Add(startPosition + i * segmentDistance * Vector3.up);
        }
    }

    private void UpdateParticles() {
        particles[0] = transform.position + anchorOffsetDistance * transform.up;

        for (int i = 1; i < particles.Count; i++) {
            Vector3 targetPosition = particles[i - 1] + transform.up * segmentDistance;
            particles[i] = Vector3.Lerp(particles[i], targetPosition, 0.1f);
        }
    }

    private void UpdateLinks() {
        for (int i = 0; i < links.Count; i++) {
            if (links[i] != null && i + 1 < particles.Count) {
                Vector3 start = particles[i];
                Vector3 end = particles[i + 1];

                links[i].transform.position = (start + end) / 2;
                if ((end - start).magnitude > 1.0001f) {
                    Quaternion lookRotation = Quaternion.LookRotation(end - start);
                    links[i].transform.rotation = Quaternion.Slerp(links[i].transform.rotation, lookRotation * Quaternion.Euler(baseLinkRotation), rotationDamp);
                }
            }
        }
    }

    private void ClearChain() {
        particles.Clear();

        for (int i = 0; i < links.Count; i++) {
            GameObject link = links[i];
            if (link != null) Destroy(link);
        }

        links.Clear();
    }

    public void IncreaseChainLength() {
        if (particles.Count < maxParticles) {
            Vector3 newParticlePosition = particles[^1] + segmentDistance * Vector3.up;
            particles.Add(newParticlePosition);
        }
    }

    public void AddCarriedObject(Rigidbody newObject) {
        if (links.Count < particles.Count - 1) {
            GameObject link = newObject.gameObject;
            links.Add(link);
        }
    }

    public void IncreaseMaxParticles(int amount) {
        maxParticles += amount;
        while (particles.Count < maxParticles) IncreaseChainLength();
    }

    public bool HasCarriedObjects() => links.Count > 0;

    public void RemoveTopCarriedObject() {
        if (links.Count > 0) {
            GameObject topLink = links[^1];
            links.RemoveAt(links.Count - 1);
            Destroy(topLink);
        }
    }

    public void RemoveAllCarriedObjects() {
        for (int i = 0; i < links.Count; i++) {
            GameObject link = links[i];
            Destroy(link);
        }

        links.Clear();
    }

    public int GetCarriedObjectCount() => links.Count;

    public bool CanCarry() => links.Count < maxParticles - 1;
}