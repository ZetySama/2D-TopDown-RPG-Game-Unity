using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum EnemyState { Patrol, Chase, Attack }
    private EnemyState currentState;

    // --- Referanslar ---
    private Transform playerTransform;
    private EnemyPathfinding pathfinding;
    private Animator myAnimator;
    private Knockback knockback;
    private Rigidbody2D rb;

    // --- Kilit Mekanizmasý ---
    private bool isAttacking = false;

    // --- AI GENEL AYARLAR ---
    [Header("Mesafe Ayarlarý")]
    [Tooltip("Düþmanýn oyuncuyu fark edeceði mesafe.")]
    [SerializeField] private float detectionRange = 10f;
    [Tooltip("Düþmanýn durup saldýracaðý (veya patlayacaðý) mesafe.")]
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private LayerMask playerLayer;

    [Header("Saldýrý Ayarlarý")]
    [Tooltip("Saldýrýlar arasý bekleme süresi.")]
    [SerializeField] private float attackCooldown = 1.5f;
    private float timeSinceLastAttack = 0f;

    [Tooltip("ÝÞARETLÝ: Saldýrýrken donar (Mantar).\nÝÞARETSÝZ: Saldýrýrken hareket edebilir (Slime).")]
    [SerializeField] private bool lockStateOnAttack = false;

    // --- SALDIRI TÝPÝNE ÖZEL AYARLAR ---
    [Header("Slime Ayarlarý (Atýlma)")]
    [SerializeField] private float lungeForce = 12f;

    [Header("Mantar Ayarlarý (Patlama)")]
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private bool drawExplosionGizmo = true;

    // --- GÜNCEL SES AYARI ---
    [Tooltip("Patlama sesini çalacak olan AudioSource bileþeni.")]
    [SerializeField] private AudioSource explosionSource;
    // ------------------------

    [Header("Hareket Ayarlarý")]
    [SerializeField] private float patrolMoveSpeed = 1f;
    [SerializeField] private float chaseMoveSpeed = 3f;
    [SerializeField] private float patrolWaitTime = 2f;
    [SerializeField] private float patrolRadius = 5f;

    [Header("Duvar & Takýlma Kontrolü")]
    [SerializeField] private float wallCheckDistance = 1f;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float maxPatrolDuration = 10f;
    private float currentPatrolTimer = 0f;

    private Vector2 startPosition;
    private Vector2 currentPatrolTarget;
    private float timeSinceArrivedAtPatrolPoint = 0f;


    void Awake()
    {
        pathfinding = GetComponent<EnemyPathfinding>();
        myAnimator = GetComponent<Animator>();
        knockback = GetComponent<Knockback>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        if (PlayerController.Instance != null)
        {
            playerTransform = PlayerController.Instance.transform;
        }

        currentState = EnemyState.Patrol;
        startPosition = transform.position;
        SetNewPatrolPoint();

        // Ýlk saldýrýya hazýr baþla (Beklemesiz)
        timeSinceLastAttack = attackCooldown;
    }

    void Update()
    {
        if (playerTransform == null) return;
        if (knockback != null && knockback.gettingKnockedBack) return;

        // KÝLÝT KONTROLÜ (Mantar saldýrýyorsa düþünmeyi durdur)
        if (isAttacking && lockStateOnAttack) return;

        // Durum Belirleme
        EnemyState previousState = currentState;
        bool isPlayerInAttackRange = Physics2D.OverlapCircle(transform.position, attackRange, playerLayer);
        bool isPlayerInDetectionRange = Physics2D.OverlapCircle(transform.position, detectionRange, playerLayer);

        if (isPlayerInAttackRange) currentState = EnemyState.Attack;
        else if (isPlayerInDetectionRange) currentState = EnemyState.Chase;
        else currentState = EnemyState.Patrol;

        // Saldýrýya Giriþ (Hareketi Durdur)
        if (currentState == EnemyState.Attack && previousState != EnemyState.Attack)
        {
            pathfinding.enabled = false;
            rb.velocity = Vector2.zero;
        }
        // Saldýrýdan Çýkýþ (Hareketi Baþlat)
        if (currentState != EnemyState.Attack && previousState == EnemyState.Attack)
        {
            pathfinding.enabled = true;
        }
    }

    void FixedUpdate()
    {
        if (playerTransform == null) return;

        // KÝLÝT KONTROLÜ (Mantar saldýrýyorsa hareket etme)
        if (isAttacking && lockStateOnAttack) return;

        switch (currentState)
        {
            case EnemyState.Patrol:
                HandlePatrol();
                break;

            case EnemyState.Chase:
                pathfinding.moveSpeed = chaseMoveSpeed;
                Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;

                // Duvar Kontrolü (Kovalarken)
                RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, wallCheckDistance, wallLayer);
                if (hit.collider != null) pathfinding.MoveTo(Vector2.zero);
                else pathfinding.MoveTo(directionToPlayer);
                break;

            case EnemyState.Attack:
                HandleAttack();
                break;
        }
    }

    // --- PATROL FONKSÝYONLARI ---
    private void HandlePatrol()
    {
        pathfinding.moveSpeed = patrolMoveSpeed;

        if (Vector2.Distance(transform.position, currentPatrolTarget) < 0.5f)
        {
            pathfinding.MoveTo(Vector2.zero);
            currentPatrolTimer = 0f;
            timeSinceArrivedAtPatrolPoint += Time.fixedDeltaTime;

            if (timeSinceArrivedAtPatrolPoint >= patrolWaitTime)
                SetNewPatrolPoint();
        }
        else
        {
            currentPatrolTimer += Time.fixedDeltaTime;
            if (currentPatrolTimer >= maxPatrolDuration)
            {
                SetNewPatrolPoint();
                return;
            }

            Vector2 direction = (currentPatrolTarget - (Vector2)transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, wallCheckDistance, wallLayer);

            if (hit.collider != null) SetNewPatrolPoint();
            else pathfinding.MoveTo(direction);
        }
    }

    private void SetNewPatrolPoint()
    {
        currentPatrolTarget = startPosition + Random.insideUnitCircle * patrolRadius;
        timeSinceArrivedAtPatrolPoint = 0f;
        currentPatrolTimer = 0f;
    }

    // --- SALDIRI YÖNETÝMÝ ---
    void HandleAttack()
    {
        timeSinceLastAttack += Time.fixedDeltaTime;

        // Süre dolduysa ve þu an zaten saldýrmýyorsak
        if (timeSinceLastAttack >= attackCooldown && !isAttacking)
        {
            timeSinceLastAttack = 0f;
            isAttacking = true;

            // Mantar için patlama kilidini aç (Slime'da yoksa hata vermez, sadece uyarý verir)
            myAnimator.SetBool("isExploding", true);

            // Her ikisi için de saldýrý animasyonunu baþlat
            myAnimator.SetTrigger("Attack");

            // Hareketi durdur
            pathfinding.enabled = false;
            rb.velocity = Vector2.zero;
        }
    }

    // SLIME ÝÇÝN: Saldýrý bitince kilidi aç (Animation Event ile çaðrýlýr)
    public void FinishAttack()
    {
        isAttacking = false;
        pathfinding.enabled = true;
    }

    // ========================================================================
    // EVENT FONKSÝYONLARI
    // ========================================================================

    // 1. SLIME ÝÇÝN: Atýlma (Lunge)
    public void ApplyAttackLunge()
    {
        if (playerTransform == null) return;
        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
        rb.AddForce(directionToPlayer * lungeForce, ForceMode2D.Impulse);
    }

    // 2. MANTAR ÝÇÝN: Patlama (Explosion) - Coroutine Baþlatýr
    public void TriggerExplosion()
    {
        StartCoroutine(ExplosionRoutine());
    }

    private IEnumerator ExplosionRoutine()
    {
        // 1. Hasar Kontrolü
        Collider2D player = Physics2D.OverlapCircle(transform.position, explosionRadius, playerLayer);
        if (player != null)
        {
            Debug.Log("<color=red>BOOM! Player hasar yedi!</color>");
            // player.GetComponent<PlayerHealth>().TakeDamage(1);
        }

        // 2. Sesi Çal
        if (explosionSource != null)
        {
            explosionSource.Play();
        }

        // 3. Mantarý Görünmez ve Etkisiz Yap (Ses Çalarken)
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        rb.simulated = false;
        // Varsa üzerindeki Canvas'ý da kapat
        var canvas = GetComponentInChildren<Canvas>();
        if (canvas) canvas.enabled = false;

        // 4. Sesin Bitmesini Bekle
        float waitTime = 0f;
        if (explosionSource != null && explosionSource.clip != null)
        {
            waitTime = explosionSource.clip.length;
        }

        // Ses uzunluðu kadar (veya en az 0.1 sn) bekle
        yield return new WaitForSeconds(Mathf.Max(waitTime, 0.1f));

        // 5. Objeyi Yok Et
        Destroy(gameObject);
    }

    // Gizmo'lar
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(startPosition, patrolRadius);

        if (drawExplosionGizmo)
        {
            Gizmos.color = new Color(1, 0.5f, 0, 0.5f);
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }

        if (Application.isPlaying && (currentState == EnemyState.Patrol || currentState == EnemyState.Chase))
        {
            Vector2 direction = Vector2.zero;
            if (currentState == EnemyState.Patrol)
                direction = (currentPatrolTarget - (Vector2)transform.position).normalized;
            else if (playerTransform != null)
                direction = (playerTransform.position - transform.position).normalized;

            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, (Vector2)transform.position + (direction * wallCheckDistance));
        }
    }
}