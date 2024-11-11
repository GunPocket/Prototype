using TMPro;
using UnityEngine;

public class Observer : MonoBehaviour {
    [SerializeField] private int initialPoints = 0;
    public static Observer Instance { get; private set; }
    private TMP_Text m_Text;

    private int points;

    public int Money {
        get { return points; }
        set {
            points = value;
            if (m_Text != null) m_Text.text = $"Points: {value}";
        }
    }

    private void Awake() {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        points = initialPoints;
        if (!GameObject.FindGameObjectWithTag("PointsText").TryGetComponent<TMP_Text>(out m_Text)) {
            Debug.LogWarning("PointsText object not found or missing TMP_Text component.");
        } else {
            m_Text.text = $"Points: {points}";
        }
    }
}