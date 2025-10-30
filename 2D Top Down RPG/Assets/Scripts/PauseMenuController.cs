using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; // <-- Butonlar i�in gerekli

public class PauseMenuController : MonoBehaviour
{
    [Tooltip("Durdurma men�s� olarak a��l�p kapanacak olan UI Paneli.")]
    [SerializeField]
    private GameObject pauseMenuPanel;

    [Tooltip("Oyunu devam ettirecek olan B�R�NC� buton.")]
    [SerializeField]
    private Button resumeButton1;

    [Tooltip("Oyunu devam ettirecek olan �K�NC� buton.")]
    [SerializeField]
    private Button resumeButton2;

    [Tooltip("Oyundan ��kartan buton.")] // Tooltip eklemek iyi bir pratik
    [SerializeField]
    private Button quitGame;

    private bool isPaused = false;
    private PlayerControls playerControls;

    void Awake()
    {
        playerControls = new PlayerControls();
    }

    void OnEnable()
    {
        playerControls.UI.Cancel.Enable();
        playerControls.UI.Cancel.performed += ctx => TogglePauseMenu();
    }

    void OnDisable()
    {
        playerControls.UI.Cancel.Disable();
        playerControls.UI.Cancel.performed -= ctx => TogglePauseMenu();
    }

    void Start()
    {
        // Pause panelini ba�lang��ta gizle
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }
        Time.timeScale = 1f;
        isPaused = false;

        // --- Resume Butonlar� ---
        if (resumeButton1 != null)
        {
            resumeButton1.onClick.AddListener(ResumeGame);
        }

        if (resumeButton2 != null)
        {
            resumeButton2.onClick.AddListener(ResumeGame);
        }

        // --- D�ZELTME 1: Do�ru fonksiyonu ba�la ---
        if (quitGame != null)
        {
            quitGame.onClick.AddListener(QuitGame); // Application.Quit de�il, QuitGame olmal�
        }
    }

    void OnDestroy()
    {
        // Dinleyicileri temizle
        if (resumeButton1 != null)
        {
            resumeButton1.onClick.RemoveListener(ResumeGame);
        }

        if (resumeButton2 != null)
        {
            resumeButton2.onClick.RemoveListener(ResumeGame);
        }

        // --- D�ZELTME 3: QuitGame dinleyicisini de kald�r ---
        if (quitGame != null)
        {
            quitGame.onClick.RemoveListener(QuitGame);
        }
    }


    public void TogglePauseMenu()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    void PauseGame()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(true);
        }
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }
        Time.timeScale = 1f;
        isPaused = false;
    }

    // --- D�ZELTME 2: Debug.Log'u ba�a al ---
    public void QuitGame()
    {
        Debug.Log("Oyun kapat�ld�."); // �NCE LOG AT
        Application.Quit(); // SONRA KAPAT
    }
}