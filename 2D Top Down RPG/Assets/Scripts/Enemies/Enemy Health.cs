using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // <-- UI (Slider) için gerekli

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 3;
    [SerializeField] private Slider healthSlider;


    [Tooltip("Ölüm animasyonunun yaklaþýk ne kadar sürdüðü (saniye).")]
    [SerializeField] private float deathAnimationTime = 1f;
    private Animator myAnimator;
    private Collider2D myCollider;
    private EnemyPathfinding pathfinding;
    private Rigidbody2D rb;
    private bool isDead = false;

    // --- SES DEÐÝÞKENLERÝ ---
    [SerializeField] private AudioSource deathsound;
    [SerializeField] private AudioSource takeDamageSound; // <-- YENÝ (Hasar alma sesini buraya atayýn)
    // -------------------------

    private int currentHealth;
    private Knockback knockback;
    private Flash flash;

    private void Awake()
    {
        knockback = GetComponent<Knockback>();
        flash = GetComponent<Flash>();
        myAnimator = GetComponent<Animator>();
        myCollider = GetComponent<Collider2D>();
        pathfinding = GetComponent<EnemyPathfinding>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        currentHealth = startingHealth;
        if (healthSlider != null)
        {
            healthSlider.maxValue = startingHealth;
            healthSlider.value = currentHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        // Eðer zaten ölüyorsa, tekrar hasar almasýn (ve ses çalmasýn)
        if (isDead) return;

        // --- YENÝ EKLENEN KISIM ---
        // Hasar aldýðýnda sesi çal
        if (takeDamageSound != null)
        {
            takeDamageSound.Play();
        }
        // -------------------------

        currentHealth -= damage;

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        if (knockback != null)
        {
            knockback.GetKnockedBack(PlayerController.Instance.transform, 15f);
        }

        StartCoroutine(flash.FlashRoutine());
    }

    public void DetectDeath()
    {
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            Debug.Log(gameObject.name + " öldü!");

            // 1. Animasyonu tetikle
            if (myAnimator) myAnimator.SetTrigger("Death");

            // 2. Ölüm sesini çal
            if (deathsound) deathsound.Play();

            // 3. Düþmaný "pasif" hale getir
            if (pathfinding) pathfinding.enabled = false;
            if (myCollider) myCollider.enabled = false;
            if (rb) rb.velocity = Vector2.zero;
            if (knockback) knockback.enabled = false;

            // 4. Can barýný gizle
            if (healthSlider != null)
            {
                healthSlider.gameObject.SetActive(false);
            }

            StartCoroutine(DestroyAfterAnimation(deathAnimationTime));
        }
    }

    private IEnumerator DestroyAfterAnimation(float animationDelay)
    {
        float soundDelay = 0f;

        // Ölüm sesi klibinin uzunluðunu al
        if (deathsound != null && deathsound.clip != null)
        {
            soundDelay = deathsound.clip.length;
        }

        // Animasyon ve ses süresinden en uzun olaný bekle
        float timeToWait = Mathf.Max(animationDelay, soundDelay);

        yield return new WaitForSeconds(timeToWait);

        // Obje bittikten sonra yok et
        Destroy(gameObject);
    }
}