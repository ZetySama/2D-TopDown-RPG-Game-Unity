using UnityEngine;
using UnityEngine.InputSystem; // Gerekli

public class PauseMenuController : MonoBehaviour
{
    [Tooltip("Durdurma men�s� olarak a��l�p kapanacak olan UI Paneli.")]
    [SerializeField]
    private GameObject pauseMenuPanel;

    private bool isPaused = false;
    private PlayerControls playerControls; // Input Actions class'�m�z�n referans�

    void Awake()
    {
        playerControls = new PlayerControls();
    }

    // Script etkinle�ti�inde �al���r
    void OnEnable()
    {
        // --- DE����KL�K BURADA ---
        // "Cancel" Action'�n� dinlemeye ba�la (UI Action Map'indeki)
        playerControls.UI.Cancel.Enable();
        playerControls.UI.Cancel.performed += ctx => TogglePauseMenu(); // Action tetiklendi�inde TogglePauseMenu'yu �a��r
        // -------------------------
    }

    // Script devre d��� kald���nda �al���r
    void OnDisable()
    {
        // --- DE����KL�K BURADA ---
        // "Cancel" Action'�n� dinlemeyi b�rak
        playerControls.UI.Cancel.Disable();
        // -------------------------
    }

    // Start, TogglePauseMenu, PauseGame, ResumeGame fonksiyonlar� ayn� kalabilir...
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
        // �ste�e ba�l�: Fareyi tekrar kilitle/gizle
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
    }
}