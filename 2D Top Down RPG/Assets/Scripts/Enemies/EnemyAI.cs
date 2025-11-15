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
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float attackRange = 4f;
    [SerializeField] private LayerMask playerLayer; //

    [Header("Saldýrý Ayarlarý")]
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float lungeForce = 5f; //
    private float timeSinceLastAttack = 0f;

    [Header("Hareket Ayarlarý")]
    [SerializeField] private float patrolMoveSpeed = 1f;
    [SerializeField] private float chaseMoveSpeed = 3f;
    [SerializeField] private float patrolWaitTime = 2f;
    [SerializeField] private float patrolRadius = 5f;

    [Header("Patrol Duvar Kontrolü")]
    [Tooltip("Düþmanýn önündeki duvarý ne kadar uzaktan fark edeceði.")]
    [SerializeField] private float wallCheckDistance = 1f;
    [Tooltip("Duvar olarak kabul edilecek katman (Muhtemelen 'Default').")]
    [SerializeField] private LayerMask wallLayer;

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


    // 2. Karar Verme (Update) - (Deðiþiklik yok)
    void Update()
    {
        if (playerTransform == null) return;
        if (knockback != null && knockback.gettingKnockedBack) //
        {
            return;
        }

        EnemyState previousState = currentState;
        bool isPlayerInAttackRange = Physics2D.OverlapCircle(transform.position, attackRange, playerLayer); //
        bool isPlayerInDetectionRange = Physics2D.OverlapCircle(transform.position, detectionRange, playerLayer); //

        if (isPlayerInAttackRange) { currentState = EnemyState.Attack; } //
        else if (isPlayerInDetectionRange) { currentState = EnemyState.Chase; } //
        else { currentState = EnemyState.Patrol; } //

        if (currentState == EnemyState.Attack && previousState != EnemyState.Attack)
        {
            pathfinding.enabled = false; //
            rb.velocity = Vector2.zero; //
        }
        if (currentState != EnemyState.Attack && previousState == EnemyState.Attack)
        {
            pathfinding.enabled = true; //
        }
    }


    // 3. Eyleme Geçme (FixedUpdate) - (Chase durumu güncellendi)
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
                pathfinding.moveSpeed = chaseMoveSpeed; //
                Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;

                // 1. Oyuncuya giden yolda duvar var mý diye kontrol et
                RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, wallCheckDistance, wallLayer);

                // 2. EÐER BÝR DUVAR ALGILANIRSA:
                if (hit.collider != null)
                {
                    // Takibi býrak ve duvara çarpma (dur)
                    pathfinding.MoveTo(Vector2.zero); //
                }
                else // 3. YOL TEMÝZSE:
                {
                    // Oyuncuyu takip et
                    pathfinding.MoveTo(directionToPlayer); //
                }
                // -------------------------
                break;

            case EnemyState.Attack:
                HandleAttack();
                break;
        }
    }

    // --- PATROL FONKSÝYONU --- (Deðiþiklik yok)
    private void HandlePatrol()
    {
        pathfinding.moveSpeed = patrolMoveSpeed; //

        if (Vector2.Distance(transform.position, currentPatrolTarget) < 0.5f)
        {
            pathfinding.MoveTo(Vector2.zero); //
            timeSinceArrivedAtPatrolPoint += Time.fixedDeltaTime; //

            if (timeSinceArrivedAtPatrolPoint >= patrolWaitTime) //
            {
                SetNewPatrolPoint(); //
            }
        }
        else
        {
            Vector2 direction = (currentPatrolTarget - (Vector2)transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, wallCheckDistance, wallLayer);

            if (hit.collider != null)
            {
                SetNewPatrolPoint(); //
            }
            else
            {
                pathfinding.MoveTo(direction); //
            }
        }
    }

    private void SetNewPatrolPoint()
    {
        currentPatrolTarget = startPosition + Random.insideUnitCircle * patrolRadius; //
        timeSinceArrivedAtPatrolPoint = 0f;
    }

    // --- DÝÐER FONKSÝYONLAR --- (Deðiþiklik yok)
    void HandleAttack()
    {
        timeSinceLastAttack += Time.fixedDeltaTime;
        if (timeSinceLastAttack >= attackCooldown) //
        {
            timeSinceLastAttack = 0f;
            myAnimator.SetTrigger("Attack"); //
        }
    }

    public void ApplyAttackLunge()
    {
        if (playerTransform == null) return;
        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
        rb.AddForce(directionToPlayer * lungeForce, ForceMode2D.Impulse); //
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(startPosition, patrolRadius);

        // Duvar sensörünü 'Chase' durumunda da göster
        if (currentState == EnemyState.Patrol || currentState == EnemyState.Chase)
        {
            Vector2 direction;
            if (currentState == EnemyState.Patrol)
                direction = (currentPatrolTarget - (Vector2)transform.position).normalized;
            else
                direction = (playerTransform.position - transform.position).normalized;

            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, (Vector2)transform.position + (direction * wallCheckDistance));
        }
    }
}