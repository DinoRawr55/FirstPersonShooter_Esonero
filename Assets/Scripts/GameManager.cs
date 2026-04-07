using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject panelMain;
    public GameObject panelSettings;
    public GameObject panelPause;
    public GameObject panelDeath;
    public GameObject panelWin;

    [Header("Win Settings")]
    public int enemiesToKill = 6;
    private int enemiesKilled = 0;

    private bool isPaused = false;
    public Slider volumeSlider;

    // Questa variabile "sopravvive" al caricamento della scena
    private static bool shouldStartInGame = false;

    void Start()
    {
        // Se abbiamo cliccato Restart, saltiamo il menu
        if (shouldStartInGame)
        {
            panelMain?.SetActive(false);
            Time.timeScale = 1f;
            UpdateCursor(false);
            shouldStartInGame = false; // Resetta per la prossima volta
        }
        else
        {
            panelMain?.SetActive(true);
            Time.timeScale = 0f;
            UpdateCursor(true);
        }

        panelSettings?.SetActive(false);
        panelPause?.SetActive(false);
        panelDeath?.SetActive(false);
        panelWin?.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (panelMain != null && !panelMain.activeSelf && !panelDeath.activeSelf && !panelWin.activeSelf)
            {
                TogglePause();
            }
        }
    }

    public void StartGame()
    {
        panelMain?.SetActive(false);
        Time.timeScale = 1f;
        UpdateCursor(false);
    }

    // --- CORREZIONE RESTART ---
    public void ReStartGameDIE() => ExecuteRestart();
    public void ReStartGameWIN() => ExecuteRestart();

    private void ExecuteRestart()
    {
        shouldStartInGame = true; // Diciamo al prossimo Start() di saltare il menu
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMenu()
    {
        shouldStartInGame = false;
        Time.timeScale = 0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Collega questi ai tasti "Torna al menu"
    public void GofromDeathtoMain() => ReturnToMenu();
    public void GofromWintoMain() => ReturnToMenu();
    public void GofromPausetoMain() => ReturnToMenu();

    // --- ALTRE FUNZIONI ---
    public void OpenSettings() { panelMain?.SetActive(false); panelSettings?.SetActive(true); }
    public void BackToMain() { panelSettings?.SetActive(false); panelMain?.SetActive(true); }
    public void QuitGame() { Application.Quit(); }

    public void TogglePause()
    {
        isPaused = !isPaused;
        panelPause?.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
        UpdateCursor(isPaused);
    }

    public void GameOver() { panelDeath?.SetActive(true); Time.timeScale = 0f; UpdateCursor(true); }
    public void EnemyKilled() { enemiesKilled++; if (enemiesKilled >= enemiesToKill) WinGame(); }
    void WinGame() { panelWin?.SetActive(true); Time.timeScale = 0f; UpdateCursor(true); }

    private void UpdateCursor(bool state)
    {
        Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = state;
    }
}