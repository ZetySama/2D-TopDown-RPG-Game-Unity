using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // <-- 1. ADIM: UI elemanlar�n� kullanmak i�in bunu ekleyin

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 3;

    // --- 2. ADIM: Slider'� Inspector'dan atamak i�in ---
    [Tooltip("D��man�n can bar�n� g�sterecek olan Slider objesi")]
    [SerializeField] private Slider healthSlider;
    // --------------------------------------------------

    private int currentHealth;

    private void Start()
    {
        currentHealth = startingHealth;

        // --- 3. ADIM: Slider'� ba�lang��ta ayarla ---
        if (healthSlider != null)
        {
            healthSlider.maxValue = startingHealth; // Slider'�n maksimum de�erini ayarla
            healthSlider.value = currentHealth;   // Slider'�n mevcut de�erini ayarla
        }
        // ---------------------------------------------
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // --- 4. ADIM: Hasar ald�k�a slider'� g�ncelle ---
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth; // Slider'�n de�erini yeni cana e�itle
        }
        // ---------------------------------------------

        Debug.Log(currentHealth);
        DetectDeath(); // Bu fonksiyonun sizde tan�ml� oldu�unu varsay�yorum
    }

    // DetectDeath fonksiyonunuzu buraya ekleyin (e�er yoksa)
    private void DetectDeath()
    {
        if (currentHealth <= 0)
        {
            // �l�m i�lemleri, �rn: Destroy(gameObject);
            Debug.Log("D��man �ld�!");
            Destroy(gameObject);
        }
    }
}