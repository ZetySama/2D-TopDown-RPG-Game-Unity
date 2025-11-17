using UnityEngine;

public class DistanceBasedAudio : MonoBehaviour
{
    // Sesi çalacak olan "hoparlör" bileþeni
    public AudioSource audioSource;

    // Mesafeyi ölçmek için oyuncunun Transform'u
    public Transform player;

    // Sesin duyulabileceði maksimum uzaklýk
    public float maxDistance = 5f;
    public float maxVolume = 1f;

    // Her frame'de çalýþýr
    void Update()
    {
        // 1. Oyuncu ile bu obje (su) arasýndaki mesafeyi hesapla
        float distance = Vector3.Distance(player.position, transform.position);

        // 2. Mesafeye göre ses seviyesini (yüzdesini) hesapla
        float volumePercent = 1f - (distance / maxDistance);

        // 3. Bulunan yüzdeyi 'maxVolume' ile çarparak nihai sesi ayarla
        audioSource.volume = Mathf.Clamp01(volumePercent * maxVolume);
    }

    // --- YENÝ EKLENEN FONKSÝYON ---
    // Bu fonksiyon SADECE Unity Editör'de çalýþýr
    // ve objeyi Sahnede seçtiðinizde görünür hale gelir.
    private void OnDrawGizmosSelected()
    {
        // 1. Gizmo'nun (çizginin) rengini beyaz yap
        Gizmos.color = Color.white;

        // 2. Bu objenin pozisyonunu (transform.position) merkez alarak,
        // 'maxDistance' yarýçapýnda bir daire (WireSphere) çiz
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }
}