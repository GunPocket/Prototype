using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour {
    [SerializeField] private StackHandler stackHandler;
    [SerializeField] private Renderer playerRenderer;
    [SerializeField] private Button increaseStackButton;

    private float currentHue = 150f / 360f;

    private void Start() {
        if (increaseStackButton == null) {
            return;
        }
        increaseStackButton.onClick.AddListener(OnIncreaseStackButtonClicked);
    }

    private void Update() {
        // Atualizar o estado do botão
        if (increaseStackButton != null && Observer.Instance != null) {
            increaseStackButton.interactable = Observer.Instance.Money >= 100;
        }
    }

    private void OnIncreaseStackButtonClicked() {
        // Diminuir o dinheiro ao clicar no botão
        if (Observer.Instance != null) {
            Observer.Instance.Money -= 100;
        }

        // Aumentar o limite da stack
        if (stackHandler == null) {
        } else
            stackHandler.IncreaseMaxParticles(1);

        // Atualizar a cor do jogador
        UpdatePlayerColor();
    }

    private void UpdatePlayerColor() {
        currentHue -= 0.1f;
        if (currentHue < 0f) currentHue += 1f;

        Color newColor = Color.HSVToRGB(currentHue, 1f, 1f);
        if (playerRenderer != null) {
            playerRenderer.material.color = newColor;
        }
    }
}