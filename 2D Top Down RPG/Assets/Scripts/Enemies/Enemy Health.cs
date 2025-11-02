using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // <-- UI (Slider) için gerekli

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 3;
    [Tooltip("Düþmanýn can barýný gösterecek olan Slider objesi")]
    [SerializeField] private Slider healthSlider;
    private int currentHealth;
    private Knockback knockback;
    private Flash flash;

    private void Awake()
    {
        knockback = GetComponent<Knockback>();
        flash = GetComponent<Flash>();
    }
  
    private void Start()
    {
        currentHealth = startingHealth;

        // Slider'ý baþlangýçta ayarla
        if (healthSlider != null)
        {
            healthSlider.maxValue = startingHealth; // Slider'ýn maksimum deðerini ayarla
            healthSlider.value = currentHealth;   // Slider'ýn mevcut deðerini ayarla
        }
    }

    public void TakeDamage(int damage)
    {
        // Caný azalt
        currentHealth -= damage;

        // Can barýný (Slider) güncelle
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth; // Slider'ýn deðerini yeni cana eþitle
        }

        // --- KNOCKBACK'Ý TETÝKLE (Resimden) ---
        // (knockback bileþeninin var olduðundan emin olarak)
        if (knockback != null)
        {
            // PlayerController'ýnýzýn 'Instance' (Singleton) kullandýðýný varsayarak
            knockback.GetKnockedBack(PlayerController.Instance.transform, 15f);
        }
        // --------------------------------------

        Debug.Log("Düþmanýn caný: " + currentHealth); // Debug mesajý
        StartCoroutine(flash.FlashRoutine());
    }

    public void DetectDeath()
    {
        if (currentHealth <= 0)
        {
            // Ölüm iþlemleri
            Debug.Log(gameObject.name + " öldü!");
            Destroy(gameObject);
        }
    }
}