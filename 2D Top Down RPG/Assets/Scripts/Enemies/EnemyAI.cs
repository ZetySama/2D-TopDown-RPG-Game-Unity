using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum EnemyState { Patrol, Chase, Attack }
    private EnemyState currentState;

    // --- Referanslar ---
    private Transform playerTransform;
    private EnemyPathfinding pathfinding; //
    private Animator myAnimator;
    private Knockback knockback; //
    private Rigidbody2D rb; //

    // --- AI Ayarlarý (Inspector'dan deðiþecek) ---
    [Header("Mesafe Ayarlarý")]
    [Tooltip("Düþmanýn oyuncuyu fark edeceði (takibe baþlayacaðý) mesafe.")]
    [SerializeField] private float detectionRange = 10f;
    [Tooltip("Düþmanýn durup saldýracaðý mesafe (detectionRange'den küçük olmalý).")]
    [SerializeField] private float attackRange = 4f; //
    [Tooltip("Sadece 'Player' katmanýný algýlamak için.")]
    [SerializeField] private LayerMask playerLayer; //

    [Header("Saldýrý Ayarlarý")]
    [Tooltip("Ýki saldýrý arasýndaki bekleme süresi (saniye).")]
    [SerializeField] private float attackCooldown = 1.5f; //
    [Tooltip("Saldýrý sýrasýnda atýlma (lunge) kuvveti.")]
    [SerializeField] private float lungeForce = 5f; //
    private float timeSinceLastAttack = 0f;

    [Header("Hareket Ayarlarý")] // <-- BAÞLIÐI GÜNCELLEDÝM
    [Tooltip("Devriye atarken kullanýlacak hareket hýzý.")]
    [SerializeField] private float patrolMoveSpeed = 1f;
    [Tooltip("Oyuncuyu kovalarken kullanýlacak hareket hýzý.")]
    [SerializeField] private float chaseMoveSpeed = 3f; // <-- YENÝ EKLENEN DEÐÝÞKEN
    [Tooltip("Bir devriye noktasýna ulaþtýktan sonra bekleme süresi.")]
    [SerializeField] private float patrolWaitTime = 2f;
    [Tooltip("Baþlangýç noktasýndan ne kadar uzaða devriye atabileceði.")]
    [SerializeField] private float patrolRadius = 5f;

    private Vector2 startPosition;
    private Vector2 currentPatrolTarget;
    private float timeSinceArrivedAtPatrolPoint = 0f;


    void Awake()
    {
        pathfinding = GetComponent<EnemyPathfinding>(); //
        myAnimator = GetComponent<Animator>();
        knockback = GetComponent<Knockback>(); //
        rb = GetComponent<Rigidbody2D>(); //
    }

    void Start()
    {
        if (PlayerController.Instance != null)
        {
            playerTransform = PlayerController.Instance.transform; //
        }
        else
        {
            Debug.LogError(gameObject.name + " objesi Player'ý bulamadý!");
        }

        currentState = EnemyState.Patrol;
        startPosition = transform.position;
        SetNewPatrolPoint();
    }


    // 2. Karar Verme (Her frame çalýþýr)
    void Update()
    {
        if (playerTransform == null) return;
        if (knockback != null && knockback.gettingKnockedBack) //
        {
            return;
        }

        EnemyState previousState = currentState;
        bool isPlayerInAttackRange = Physics2D.OverlapCircle(transform.position, attackRange, playerLayer);
        bool isPlayerInDetectionRange = Physics2D.OverlapCircle(transform.position, detectionRange, playerLayer);

        if (isPlayerInAttackRange)
        {
            currentState = EnemyState.Attack;
        }
        else if (isPlayerInDetectionRange)
        {
            currentState = EnemyState.Chase;
        }
        else
        {
            currentState = EnemyState.Patrol;
        }

        // Durum geçiþini kontrol et
        if (currentState == EnemyState.Attack && previousState != EnemyState.Attack)
        {
            pathfinding.enabled = false;
            rb.velocity = Vector2.zero;
        }
        if (currentState != EnemyState.Attack && previousState == EnemyState.Attack)
        {
            pathfinding.enabled = true;
        }
    }


    // 3. Eyleme Geçme (Her fizik güncellemesinde çalýþýr)
    void FixedUpdate()
    {
        if (playerTransform == null) return;

        switch (currentState)
        {
            case EnemyState.Patrol:
                HandlePatrol();
                break;

            case EnemyState.Chase:
                // --- GÜNCELLEME BURADA ---
                pathfinding.moveSpeed = chaseMoveSpeed; // Sabit '3f' yerine deðiþkeni kullan
                // -------------------------
                Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
                pathfinding.MoveTo(directionToPlayer); //
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
            pathfinding.MoveTo(Vector2.zero); //
            timeSinceArrivedAtPatrolPoint += Time.fixedDeltaTime;

            if (timeSinceArrivedAtPatrolPoint >= patrolWaitTime)
            {
                SetNewPatrolPoint();
            }
        }
        else
        {
            Vector2 direction = (currentPatrolTarget - (Vector2)transform.position).normalized;
            pathfinding.MoveTo(direction); //
        }
    }

    private void SetNewPatrolPoint()
    {
        currentPatrolTarget = startPosition + Random.insideUnitCircle * patrolRadius;
        timeSinceArrivedAtPatrolPoint = 0f;
    }
    // ------------------------------------

    void HandleAttack()
    {
        timeSinceLastAttack += Time.fixedDeltaTime;
        if (timeSinceLastAttack >= attackCooldown)
        {
            timeSinceLastAttack = 0f;
            myAnimator.SetTrigger("Attack"); //
        }
    }

    // Animasyon Olayý (Event) tarafýndan çaðrýlýr
    public void ApplyAttackLunge()
    {
        if (playerTransform == null) return;
        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
        rb.AddForce(directionToPlayer * lungeForce, ForceMode2D.Impulse); //
    }

    // Gizmo'lar (Sahnede alanlarý görmek için)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(startPosition, patrolRadius);
    }
}