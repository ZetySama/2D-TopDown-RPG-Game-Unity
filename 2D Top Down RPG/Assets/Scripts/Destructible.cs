using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Fýçý, kutu, vazo gibi kýrýlabilir tüm objelerde bu script'i kullanabilirsin.
public class Destructible : MonoBehaviour
{
    [Header("Görsel & Animasyon")]
    [Tooltip("Animator'de oynatýlacak olan kýrýlma animasyon klibi.")]
    // --- GÜNCELLEME BURADA ---
    // String (metin) yerine doðrudan klip referansý alýyoruz
    [SerializeField] private AnimationClip breakAnimation;
    // -------------------------

    [Header("Ses Efekti")]
    [Tooltip("Obje kýrýldýðýnda çalacak olan AudioSource.")]
    [SerializeField] private AudioSource breakSound;

    private Animator myAnimator;
    private BoxCollider2D solidCollider; // Katý olan collider
    private bool isBroken = false;
    private float animationLength; // Animasyon süresini saklamak için

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        solidCollider = GetComponent<BoxCollider2D>();

        // --- GÜNCELLEME BURADA ---
        // Artýk animasyonu aramamýza gerek yok, doðrudan süresini alýyoruz
        if (breakAnimation != null)
        {
            animationLength = breakAnimation.length;
        }
        else
        {
            Debug.LogWarning(gameObject.name + ": 'Destructible' script'ine bir 'Break Animation' klibi atanmamýþ!");
            animationLength = 0.5f; // Hata olmasýn diye varsayýlan bir deðer ata
        }
        // -------------------------
    }

    // Bu fonksiyon, "Is Trigger = true" olan collider tarafýndan çaðrýlýr
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isBroken) return;

        if (other.gameObject.GetComponent<DamageSource>()) //
        {
            isBroken = true;
            myAnimator.SetTrigger("Break");

            if (solidCollider != null)
            {
                solidCollider.enabled = false;
            }

            if (breakSound != null)
            {
                breakSound.Play();
            }

            StartCoroutine(DestroyAfterDelay());
        }
    }

    // --- KORUTÝN (Deðiþiklik yok, 'animationLength' kullanýyor) ---
    private IEnumerator DestroyAfterDelay()
    {
        float soundDelay = 0f;

        if (breakSound != null && breakSound.clip != null)
        {
            soundDelay = breakSound.clip.length;
        }

        // Animasyon süresi (artýk Inspector'dan geliyor) ve ses süresinden en uzun olaný seç
        float timeToWait = Mathf.Max(animationLength, soundDelay);

        yield return new WaitForSeconds(timeToWait);

        Destroy(gameObject);
    }
}