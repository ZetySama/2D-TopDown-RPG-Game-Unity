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
        // Yakýnsa 1.0 (örn: 1 - 0/5 = 1.0)
        // Uzaksa 0.0 (örn: 1 - 5/5 = 0.0)
        float volumePercent = 1f - (distance / maxDistance);

        // 3. Bulunan yüzdeyi 'maxVolume' ile çarparak nihai sesi ayarla
        // ve Mathf.Clamp01 ile 0-1 aralýðýnda kalmasýný garantile.
        audioSource.volume = Mathf.Clamp01(volumePercent * maxVolume);
    }
}