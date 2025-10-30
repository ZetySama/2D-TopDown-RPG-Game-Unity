using UnityEngine;

public class CursorController : MonoBehaviour
{
    [Tooltip("Kullanmak istedi�imiz �zel imle� g�rseli (Texture2D).")]
    [SerializeField]
    private Texture2D cursorTexture;

    [Tooltip("�mlecin 't�klama noktas�n�' belirler. (0,0) sol �st k��edir.")]
    [SerializeField]
    private Vector2 hotspot = Vector2.zero;

    // Oyun ba�larken SADECE B�R KEZ �al���r
    void Start()
    {
        // �zel imlecimizi ayarla
        // CursorMode.Auto: M�mk�nse donan�m (h�zl�), de�ilse yaz�l�m (yava�) imleci kullan�r.
        Cursor.SetCursor(cursorTexture, hotspot, CursorMode.Auto);
    }

    // (�ste�e ba�l�) Oyundan ��karken veya ba�ka bir sahnede
    // imleci varsay�lana d�nd�rmek isterseniz:
    /*
    private void OnDisable()
    {
        // �mleci i�letim sisteminin varsay�lan imlecine d�nd�r
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
    */
}