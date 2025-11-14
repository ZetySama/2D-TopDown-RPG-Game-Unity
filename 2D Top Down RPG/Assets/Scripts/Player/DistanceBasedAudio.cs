using UnityEngine;

public class DistanceBasedAudio : MonoBehaviour
{
    // Sesi çalacak olan "hoparlör" bileþeni
    public AudioSource audioSource;

    // Mesafeyi ölçmek için oyuncunun Transform'u
    public Transform player;

    // Sesin duyulabileceði maksimum uzaklýk
    public float maxDistance = 5f;

    // Her frame'de çalýþýr
    void Update()
    {
        // 1. Oyuncu ile bu obje (su) arasýndaki mesafeyi hesapla
        float distance = Vector3.Distance(player.position, transform.position);

        // 2. Mesafeye göre ses seviyesini hesapla (0.0 ile 1.0 arasýnda)
        // Oyuncu yakýndayken (mesafe < maxDistance) ses 1'e yaklaþýr.
        // Oyuncu uzaktayken (mesafe > maxDistance) ses 0'a yaklaþýr.
        float volume = Mathf.Clamp01((float)1 - (distance / maxDistance));

        // 3. Hesaplanan ses seviyesini AudioSource'a uygula
        audioSource.volume = volume;
    }
}