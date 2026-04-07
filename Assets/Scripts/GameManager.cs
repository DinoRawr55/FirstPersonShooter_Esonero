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



    private bool isPaused = false;   //tiene traccia della pausa

    public Slider volumeSlider;  //aggiungi lo slider per il volume

    void Start()
    {
        //all avvio mostra solo il menu principale
        panelMain?.SetActive(true);
        panelSettings?.SetActive(false);
        panelPause?.SetActive(false);
        panelDeath?.SetActive(false);
        panelWin?.SetActive(false);  // Nascondi la schermata di vittoria inizialmente


        Time.timeScale = 0f;

        //mostra il cursore per cliccare i bottoni
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;


       
    }

    void Update()
    {
        //esc pausa
        if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 1f && (panelDeath == null || !panelDeath.activeSelf))
        {
            TogglePause();
        }
    }

    //start menu princ
    public void StartGame()
    {
        panelMain?.SetActive(false);
        Time.timeScale = 1f;

        //cursore al centro
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


    }

    //quit
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Sei uscito dal gioco");
    }

    //settings
    public void OpenSettings()
    {
        panelMain?.SetActive(false);
        panelSettings?.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // BACK dal settings al menu principale
    public void BackToMain()
    {
        panelSettings?.SetActive(false);
        panelMain?.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // BACK dal death to main menu
    public void GofromDeathtoMain()
    {
        panelDeath?.SetActive(false);
        panelMain?.SetActive(true);

        //resetta tutto
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // BACK dal win to main menu
    public void GofromWintoMain()
    {
        panelWin?.SetActive(false);
        panelMain?.SetActive(true);

        //resetta tutto
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    //pausa
    public void TogglePause()
    {
        if (isPaused)
        {
            panelPause?.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;

            //blocca il cursore
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            panelPause?.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;

            //mostra il cursore
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    //BACK dal pause to main menu
    public void GofromPausetoMain()
    {
        panelPause?.SetActive(false);
        panelMain?.SetActive(true);

        //ricarica la scena 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    //gameover
    public void GameOver()
    {
        panelDeath?.SetActive(true);
        Time.timeScale = 0f;

        //mostra il cursore
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        
    }

    //morte nemici
    public void EnemyKilled()
    {
        enemiesKilled++;
        Debug.Log("Nemici uccisi: " + enemiesKilled);

        if (enemiesKilled >= enemiesToKill)
        {
            WinGame();
        }
    }

    //vittoria
    void WinGame()
    {
        panelWin?.SetActive(true);
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;


        Debug.Log("HAI VINTO!");

      
    }
    public void ReStartGameDIE()
    {
        panelDeath?.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;

        //cursore al centro
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


    }
    public void ReStartGameWIN()
    {
        panelWin?.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;

        //cursore al centro
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


    }
}


