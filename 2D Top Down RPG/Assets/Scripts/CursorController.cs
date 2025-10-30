using UnityEngine;

public class CursorController : MonoBehaviour
{
    [Tooltip("Kullanmak istediðimiz özel imleç görseli (Texture2D).")]
    [SerializeField]
    private Texture2D cursorTexture;

    [Tooltip("Ýmlecin 'týklama noktasýný' belirler. (0,0) sol üst köþedir.")]
    [SerializeField]
    private Vector2 hotspot = Vector2.zero;

    // Oyun baþlarken SADECE BÝR KEZ çalýþýr
    void Start()
    {
        // Özel imlecimizi ayarla
        // CursorMode.Auto: Mümkünse donaným (hýzlý), deðilse yazýlým (yavaþ) imleci kullanýr.
        Cursor.SetCursor(cursorTexture, hotspot, CursorMode.Auto);
    }

    // (Ýsteðe baðlý) Oyundan çýkarken veya baþka bir sahnede
    // imleci varsayýlana döndürmek isterseniz:
    /*
    private void OnDisable()
    {
        // Ýmleci iþletim sisteminin varsayýlan imlecine döndür
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
    */
}