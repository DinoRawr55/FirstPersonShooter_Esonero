using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Stats")]
    public float maxHealth = 100f;
    public float health;

    [Header("UI")]
    public Slider healthSlider;   // barra vita
    public GameObject deathUI;    // canvas "Sei morto"
    public GameManager gameManager; // Trascinalo nell'inspector o cercalo in Start

    void Start()
    {
        health = maxHealth;
        if (gameManager == null) gameManager = Object.FindAnyObjectByType<GameManager>();

        if (healthSlider != null)
            healthSlider.value = health / maxHealth;

        if (deathUI != null)
            deathUI.SetActive(false);
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        if (healthSlider != null)
        {
            healthSlider.value = Mathf.Clamp01(health / maxHealth);
        }


        Debug.Log("Sei stato attaccato! Vita Player: " + health);

        if (health <= 1)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Sei morto!");
        if (gameManager != null)
        {
            gameManager.GameOver(); // Usa la funzione centralizzata del Manager
        }
        else
        {
            // Fallback se non hai il manager
            if (deathUI != null) deathUI.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}

