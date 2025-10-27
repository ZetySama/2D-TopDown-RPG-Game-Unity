using UnityEngine;
using UnityEngine.InputSystem; // Gerekli

public class PauseMenuController : MonoBehaviour
{
    [Tooltip("Durdurma menüsü olarak açýlýp kapanacak olan UI Paneli.")]
    [SerializeField]
    private GameObject pauseMenuPanel;

    private bool isPaused = false;
    private PlayerControls playerControls; // Input Actions class'ýmýzýn referansý

    void Awake()
    {
        playerControls = new PlayerControls();
    }

    // Script etkinleþtiðinde çalýþýr
    void OnEnable()
    {
        // --- DEÐÝÞÝKLÝK BURADA ---
        // "Cancel" Action'ýný dinlemeye baþla (UI Action Map'indeki)
        playerControls.UI.Cancel.Enable();
        playerControls.UI.Cancel.performed += ctx => TogglePauseMenu(); // Action tetiklendiðinde TogglePauseMenu'yu çaðýr
        // -------------------------
    }

    // Script devre dýþý kaldýðýnda çalýþýr
    void OnDisable()
    {
        // --- DEÐÝÞÝKLÝK BURADA ---
        // "Cancel" Action'ýný dinlemeyi býrak
        playerControls.UI.Cancel.Disable();
        // -------------------------
    }

    // Start, TogglePauseMenu, PauseGame, ResumeGame fonksiyonlarý ayný kalabilir...
    void Start()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }
        Time.timeScale = 1f;
        isPaused = false;
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
        // Ýsteðe baðlý: Fareyi tekrar kilitle/gizle
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
    }
}