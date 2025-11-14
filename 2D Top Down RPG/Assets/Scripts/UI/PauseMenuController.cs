using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// Bu script'in olduðu objeye bir AudioSource bileþeni eklemeyi zorunlu kýlar
[RequireComponent(typeof(AudioSource))]
public class PauseMenuController : MonoBehaviour
{
    [Tooltip("Durdurma menüsü olarak açýlýp kapanacak olan UI Paneli.")]
    [SerializeField]
    private GameObject pauseMenuPanel;

    // --- BUTONLARINIZ ---
    [SerializeField] private Button resumeButton1;
    [SerializeField] private Button resumeButton2;
    [SerializeField] private Button quitGame;

    // --- YENÝ MÜZÝK ALANLARI ---
    [Tooltip("Duraklatma menüsü açýldýðýnda çalacak olan müzik klibi.")]
    [SerializeField]
    private AudioClip pauseMusicClip;

    // Bu script'in AudioSource'u, pause müziðini çalmak için kullanýlacak
    private AudioSource pauseAudioSource;
    // ----------------------------

    private bool isPaused = false;
    private PlayerControls playerControls;

    void Awake()
    {
        playerControls = new PlayerControls();

        // --- YENÝ MÜZÝK AYARI ---
        // Bu objenin üzerindeki AudioSource bileþenini al
        pauseAudioSource = GetComponent<AudioSource>();

        // Bu AudioSource'un, oyun durduðunda bile çalýþmasýný saðla
        // (AudioListener.pause komutundan etkilenmeyecek)
        pauseAudioSource.ignoreListenerPause = true;

        // Müziðin döngüye girmesini (sürekli çalmasýný) saðla
        pauseAudioSource.loop = true;

        // Inspector'dan atadýðýmýz klibi bu AudioSource'a ata
        if (pauseMusicClip != null)
        {
            pauseAudioSource.clip = pauseMusicClip;
        }
        // -------------------------
    }

    // OnEnable, OnDisable, Start, OnDestroy metotlarýnýz ayný
    #region Standart Metotlar
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
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }
        Time.timeScale = 1f;
        isPaused = false;

        if (resumeButton1 != null) { resumeButton1.onClick.AddListener(ResumeGame); }
        if (resumeButton2 != null) { resumeButton2.onClick.AddListener(ResumeGame); }
        if (quitGame != null) { quitGame.onClick.AddListener(QuitGame); }
    }

    void OnDestroy()
    {
        if (resumeButton1 != null) { resumeButton1.onClick.RemoveListener(ResumeGame); }
        if (resumeButton2 != null) { resumeButton2.onClick.RemoveListener(ResumeGame); }
        if (quitGame != null) { quitGame.onClick.RemoveListener(QuitGame); }
    }
    #endregion

    // --- ASIL DEÐÝÞÝKLÝKLER BU ÝKÝ METOTTA ---

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
        // Eski kodunuz (Paneli aç, zamaný durdur)
        if (pauseMenuPanel != null) { pauseMenuPanel.SetActive(true); }
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // --- YENÝ SES KONTROLÜ ---
        // Sahnedeki (ignoreListenerPause = false olan) TÜM sesleri durdur
        AudioListener.pause = true;

        // Sadece pause müziðini çal
        if (pauseAudioSource != null && pauseMusicClip != null)
        {
            pauseAudioSource.Play();
        }
        // ----------------------------
    }

    public void ResumeGame()
    {
        // Eski kodunuz (Paneli kapa, zamaný baþlat)
        if (pauseMenuPanel != null) { pauseMenuPanel.SetActive(false); }
        Time.timeScale = 1f;
        isPaused = false;

        // --- YENÝ SES KONTROLÜ ---
        // Durdurulan tüm sahne seslerini devam ettir
        AudioListener.pause = false;

        // Pause müziðini durdur
        if (pauseAudioSource != null)
        {
            pauseAudioSource.Stop();
        }
        // ----------------------------
    }

    public void QuitGame()
    {
        // ÖNEMLÝ: Oyundan çýkmadan önce sesleri ve zamaný normale döndür
        AudioListener.pause = false;
        Time.timeScale = 1f;

        Debug.Log("Oyun kapatýldý.");
        Application.Quit();
    }
}