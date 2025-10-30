using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; // <-- Butonlar için gerekli

public class PauseMenuController : MonoBehaviour
{
    [Tooltip("Durdurma menüsü olarak açýlýp kapanacak olan UI Paneli.")]
    [SerializeField]
    private GameObject pauseMenuPanel;

    [Tooltip("Oyunu devam ettirecek olan BÝRÝNCÝ buton.")]
    [SerializeField]
    private Button resumeButton1;

    [Tooltip("Oyunu devam ettirecek olan ÝKÝNCÝ buton.")]
    [SerializeField]
    private Button resumeButton2;

    [Tooltip("Oyundan çýkartan buton.")] // Tooltip eklemek iyi bir pratik
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
        // Pause panelini baþlangýçta gizle
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }
        Time.timeScale = 1f;
        isPaused = false;

        // --- Resume Butonlarý ---
        if (resumeButton1 != null)
        {
            resumeButton1.onClick.AddListener(ResumeGame);
        }

        if (resumeButton2 != null)
        {
            resumeButton2.onClick.AddListener(ResumeGame);
        }

        // --- DÜZELTME 1: Doðru fonksiyonu baðla ---
        if (quitGame != null)
        {
            quitGame.onClick.AddListener(QuitGame); // Application.Quit deðil, QuitGame olmalý
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

        // --- DÜZELTME 3: QuitGame dinleyicisini de kaldýr ---
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

    // --- DÜZELTME 2: Debug.Log'u baþa al ---
    public void QuitGame()
    {
        Debug.Log("Oyun kapatýldý."); // ÖNCE LOG AT
        Application.Quit(); // SONRA KAPAT
    }
}