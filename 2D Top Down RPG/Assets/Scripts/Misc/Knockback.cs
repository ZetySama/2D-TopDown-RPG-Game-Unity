using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// HATA BURADAYDI: "KnockBack" idi, "Knockback" olmalý (küçük 'b')
public class Knockback : MonoBehaviour
{
    // Dýþarýdan okunabilen, ancak sadece bu script tarafýndan ayarlanabilen bir özellik (property).
    // Diðer script'ler (mesela PlayerMovement) bu objenin þu an geri tepme yaþayýp yaþamadýðýný
    // buradan kontrol edebilir.
    public bool gettingKnockedBack { get; private set; }

    [SerializeField] private float knockBackTime = .2f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    public void GetKnockedBack(Transform damageSource, float knockBackThrust)
    {
        gettingKnockedBack = true;

        // Geri tepme yönünü ve kuvvetini hesapla
        // (Bizim Pozisyonumuz - Hasar Verenin Pozisyonu) = Hasar verenden uzaða doðru bir yön vektörü
        Vector2 difference = (transform.position - damageSource.position).normalized * knockBackThrust * rb.mass;

        // Anlýk bir kuvvet uygula (Impulse)
        rb.AddForce(difference, ForceMode2D.Impulse);

        // Geri tepmeyi durduracak olan Coroutine'i (zamanlayýcýyý) baþlat
        StartCoroutine(KnockRoutine());
    }

    private IEnumerator KnockRoutine()
    {
        // Belirlenen 'knockBackTime' süresi kadar (0.2 saniye) bekle
        yield return new WaitForSeconds(knockBackTime);

        // Süre dolunca, geri tepmeden kaynaklanan tüm hýzý sýfýrla (kaymayý durdur)
        rb.velocity = Vector2.zero;

        // Durumu "geri tepmiyor" olarak güncelle
        gettingKnockedBack = false;
    }
}