using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; // <-- Butonlar için gerekli

public class PauseMenuController : MonoBehaviour
{
    [Tooltip("Durdurma menüsü olarak açýlýp kapanacak olan UI Paneli.")]
    [SerializeField]
    private GameObject pauseMenuPanel;

    // --- 1. SADECE ÝKÝ BUTON ALANI ---
    // Bu iki buton da ayný iþi yapacak: ResumeGame()
    [Tooltip("Oyunu devam ettirecek olan BÝRÝNCÝ buton.")]
    [SerializeField]
    private Button resumeButton1;

    [Tooltip("Oyunu devam ettirecek olan ÝKÝNCÝ buton.")]
    [SerializeField]
    private Button resumeButton2;
    // ------------------------------------------

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

        // --- 2. ÝKÝ BUTONA DA AYNI DÝNLEYÝCÝYÝ EKLEME ---
        if (resumeButton1 != null)
        {
            resumeButton1.onClick.AddListener(ResumeGame);
        }

        if (resumeButton2 != null)
        {
            resumeButton2.onClick.AddListener(ResumeGame);
        }
        // ---------------------------------------------------------
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

    // Bu fonksiyon artýk her iki buton tarafýndan da çaðrýlýyor.
    public void ResumeGame()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }
        Time.timeScale = 1f;
        isPaused = false;
        // Ýsteðe baðlý olarak fareyi burada tekrar gizleyebilirsiniz
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
    }

    // --- 3. QuitToMainMenu ve QuitGame fonksiyonlarý SÝLÝNDÝ ---
}