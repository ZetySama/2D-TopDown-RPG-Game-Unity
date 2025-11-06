using UnityEngine;
using UnityEngine.UI;

public class MinimapController : MonoBehaviour
{
    [Tooltip("Takip edilecek oyuncunun Transform bileþeni.")]
    public Transform playerTransform; //

    [Tooltip("Harita üzerinde hareket edecek olan oyuncu UI ikonu.")]
    public RectTransform playerIcon; //

    [Header("Map Boundaries")]
    [Tooltip("Dünya haritasýnýn oyun içindeki X eksenindeki minimum koordinatý.")]
    [SerializeField] private float mapWorldMinX; //

    [Tooltip("Dünya haritasýnýn oyun içindeki X eksenindeki maksimum koordinatý.")]
    [SerializeField] private float mapWorldMaxX; //

    [Tooltip("Dünya haritasýnýn oyun içindeki Y eksenindeki minimum koordinatý.")]
    [SerializeField] private float mapWorldMinY; //

    [Tooltip("Dünya haritasýnýn oyun içindeki Y eksenindeki maksimum koordinatý.")]
    [SerializeField] private float mapWorldMaxY; //

    // --- YENÝ EKLENEN KISIM (DEBUG) ---
    [Header("Debug")]
    [Tooltip("Oyun çalýþýrken harita sýnýrlarýný otomatik olarak bulmak için bunu iþaretleyin. " +
             "Konsolu izleyin, sonra oyunu durdurup deðerleri yukarýya manuel girin.")]
    [SerializeField] private bool findBoundaries = false;
    // ------------------------------------

    private RectTransform minimapRect;
    private float mapUiWidth;
    private float mapUiHeight;
    private bool boundariesHaveChanged; // Sadece konsolu spamlamamak için

    void Start()
    {
        // Player_Icon'un parent'ý olan 'Mini Map BackGround'un RectTransform'unu al
        minimapRect = playerIcon.parent.GetComponent<RectTransform>(); //
        mapUiWidth = minimapRect.rect.width;
        mapUiHeight = minimapRect.rect.height;

        if (playerTransform == null)
        {
            if (PlayerController.Instance != null)
            {
                playerTransform = PlayerController.Instance.transform;
            }
            else
            {
                Debug.LogError("Minimap: Player Transform bulunamadý!");
            }
        }

        // --- YENÝ EKLENEN KISIM ---
        // Sýnýr bulma modu aktifse ve player varsa, sýnýrlarý oyuncunun mevcut konumuyla baþlat.
        if (findBoundaries && playerTransform != null)
        {
            mapWorldMinX = playerTransform.position.x;
            mapWorldMaxX = playerTransform.position.x;
            mapWorldMinY = playerTransform.position.y;
            mapWorldMaxY = playerTransform.position.y;
            Debug.LogWarning("Minimap: SINIR BULMA MODU AKTÝF. " +
                             "Haritayý gezin ve konsoldaki deðerleri not alýn.");
        }
        // -------------------------
    }

    void LateUpdate()
    {
        if (playerTransform == null || playerIcon == null) return;

        // --- YENÝ EKLENEN KISIM ---
        // Sýnýr bulma modu aktifse, sýnýrlarý sürekli güncelle.
        if (findBoundaries)
        {
            UpdateMapBoundaries();
        }
        // -------------------------

        // --- BU KISIM HÝÇ DEÐÝÞMEDÝ ---
        // Oyuncunun dünya pozisyonunu, harita sýnýrlarý içinde 0 ile 1 arasýnda bir yüzdeye çevir
        float percentX = Mathf.InverseLerp(mapWorldMinX, mapWorldMaxX, playerTransform.position.x);
        float percentY = Mathf.InverseLerp(mapWorldMinY, mapWorldMaxY, playerTransform.position.y);

        // Bu yüzdeyi, mini map UI'ýn geniþliði ve yüksekliði ile çarparak
        // ikonun olmasý gereken yerel pozisyonunu bul
        float iconPosX = percentX * mapUiWidth;
        float iconPosY = percentY * mapUiHeight;

        // Player_Icon'un pozisyonunu güncelle
        // DÝKKAT: Bu kod, Player_Icon'un Anchor ve Pivot'unun (0,0) (sol alt) 
        // olarak ayarlandýðýný varsayar (Bir önceki mesajda düzelttiðimiz gibi).
        playerIcon.anchoredPosition = new Vector2(iconPosX, iconPosY);
        // ---------------------------------
    }

    // --- YENÝ EKLENEN FONKSÝYON ---
    /// <summary>
    /// Oyuncunun pozisyonunu izler ve harita sýnýrlarýný (Min/Max X/Y) 
    /// gerektiðinde güncelleyip konsola yazdýrýr.
    /// </summary>
    private void UpdateMapBoundaries()
    {
        boundariesHaveChanged = false; // Deðiþiklik bayraðýný sýfýrla

        // X Min
        if (playerTransform.position.x < mapWorldMinX)
        {
            mapWorldMinX = playerTransform.position.x;
            boundariesHaveChanged = true;
        }
        // X Max
        if (playerTransform.position.x > mapWorldMaxX)
        {
            mapWorldMaxX = playerTransform.position.x;
            boundariesHaveChanged = true;
        }
        // Y Min
        if (playerTransform.position.y < mapWorldMinY)
        {
            mapWorldMinY = playerTransform.position.y;
            boundariesHaveChanged = true;
        }
        // Y Max
        if (playerTransform.position.y > mapWorldMaxY)
        {
            mapWorldMaxY = playerTransform.position.y;
            boundariesHaveChanged = true;
        }

        // Eðer bu frame'de sýnýrlardan herhangi biri deðiþtiyse, KONSOLA YAZDIR
        if (boundariesHaveChanged)
        {
            // Konsolun spam ile dolmamasý için sadece 'Warning' olarak yazdýr
            Debug.LogWarning($"YENÝ HARÝTA SINIRLARI: \n" +
                             $"Min X: {mapWorldMinX}\n" +
                             $"Max X: {mapWorldMaxX}\n" +
                             $"Min Y: {mapWorldMinY}\n" +
                             $"Max Y: {mapWorldMaxY}");
        }
    }
}