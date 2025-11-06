using UnityEngine;
using UnityEngine.UI;

public class MinimapController : MonoBehaviour
{
    [Tooltip("Takip edilecek oyuncunun Transform bileþeni.")]
    public Transform playerTransform;

    [Tooltip("Harita üzerinde hareket edecek olan oyuncu UI ikonu.")]
    public RectTransform playerIcon;

    [Tooltip("Dünya haritasýnýn oyun içindeki X eksenindeki minimum koordinatý.")]
    [SerializeField] private float mapWorldMinX;

    [Tooltip("Dünya haritasýnýn oyun içindeki X eksenindeki maksimum koordinatý.")]
    [SerializeField] private float mapWorldMaxX;

    [Tooltip("Dünya haritasýnýn oyun içindeki Y eksenindeki minimum koordinatý.")]
    [SerializeField] private float mapWorldMinY;

    [Tooltip("Dünya haritasýnýn oyun içindeki Y eksenindeki maksimum koordinatý.")]
    [SerializeField] private float mapWorldMaxY;

    // Mini map UI elemanýnýn (Map_Background) RectTransform'u
    private RectTransform minimapRect;
    private float mapUiWidth;
    private float mapUiHeight;

    void Start()
    {
        // Map_Background'un RectTransform'unu al (Player_Icon'un parent'ý)
        minimapRect = playerIcon.parent.GetComponent<RectTransform>();
        mapUiWidth = minimapRect.rect.width;
        mapUiHeight = minimapRect.rect.height;

        if (playerTransform == null)
        {
            // Eðer playerTransform Inspector'dan atanmadýysa,
            // PlayerController'ý (Singleton) kullanarak bulmayý dene.
            if (PlayerController.Instance != null)
            {
                playerTransform = PlayerController.Instance.transform;
            }
            else
            {
                Debug.LogError("Minimap: Player Transform bulunamadý!");
            }
        }
    }

    // LateUpdate, player'ýn kendi hareketi (Update/FixedUpdate) bittikten sonra
    // çalýþtýðý için kamera ve UI takibi için idealdir.
    void LateUpdate()
    {
        if (playerTransform == null || playerIcon == null) return;

        // Oyuncunun dünya pozisyonunu, harita sýnýrlarý içinde 0 ile 1 arasýnda bir yüzdeye çevir
        float percentX = Mathf.InverseLerp(mapWorldMinX, mapWorldMaxX, playerTransform.position.x);
        float percentY = Mathf.InverseLerp(mapWorldMinY, mapWorldMaxY, playerTransform.position.y);

        // Bu yüzdeyi, mini map UI'ýn geniþliði ve yüksekliði ile çarparak
        // ikonun olmasý gereken yerel pozisyonunu bul
        float iconPosX = percentX * mapUiWidth;
        float iconPosY = percentY * mapUiHeight;

        // Player_Icon'un pozisyonunu güncelle
        // (Player_Icon'un 'Map_Background'un alt objesi olduðunu ve
        // Map_Background'un pivotunun (0.5, 0.5) olduðunu varsayýyoruz.
        // Eðer ikon tam ortalanmazsa, pivot ayarlarýyla oynamak gerekebilir)

        // Genellikle ikonun pivot'u (0.5, 0.5) ve anchor'larý ortada (0.5, 0.5) ise
        // bu kod, map'in sol alt köþesinden sað üst köþesine doðru düzgün çalýþýr.
        // Ýkonun anchor'larýný (Min/Max) (0, 0) yaparak sol alta sabitlemek daha garanti olabilir.
        playerIcon.anchoredPosition = new Vector2(iconPosX, iconPosY);
    }
}