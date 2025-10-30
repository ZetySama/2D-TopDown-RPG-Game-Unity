using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // <-- 1. ADIM: UI elemanlarýný kullanmak için bunu ekleyin

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 3;

    // --- 2. ADIM: Slider'ý Inspector'dan atamak için ---
    [Tooltip("Düþmanýn can barýný gösterecek olan Slider objesi")]
    [SerializeField] private Slider healthSlider;
    // --------------------------------------------------

    private int currentHealth;

    private void Start()
    {
        currentHealth = startingHealth;

        // --- 3. ADIM: Slider'ý baþlangýçta ayarla ---
        if (healthSlider != null)
        {
            healthSlider.maxValue = startingHealth; // Slider'ýn maksimum deðerini ayarla
            healthSlider.value = currentHealth;   // Slider'ýn mevcut deðerini ayarla
        }
        // ---------------------------------------------
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // --- 4. ADIM: Hasar aldýkça slider'ý güncelle ---
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth; // Slider'ýn deðerini yeni cana eþitle
        }
        // ---------------------------------------------

        Debug.Log(currentHealth);
        DetectDeath(); // Bu fonksiyonun sizde tanýmlý olduðunu varsayýyorum
    }

    // DetectDeath fonksiyonunuzu buraya ekleyin (eðer yoksa)
    private void DetectDeath()
    {
        if (currentHealth <= 0)
        {
            // Ölüm iþlemleri, örn: Destroy(gameObject);
            Debug.Log("Düþman öldü!");
            Destroy(gameObject);
        }
    }
}