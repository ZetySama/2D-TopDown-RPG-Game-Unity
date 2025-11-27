using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    // Inspector'da gizle ama diðer script'lerin eriþimine açýk tut
    [HideInInspector] public float moveSpeed;

    public enum FacingDirection { Left, Right }

    [Header("Sprite Ayarlarý")]
    [Tooltip("Karakterin orijinal resmi (PNG) hangi yöne bakýyor?")]
    public FacingDirection originalFacingDirection = FacingDirection.Left;

    private Rigidbody2D rb;
    private Vector2 moveDir;
    private Knockback knockback;

    private void Awake()
    {
        knockback = GetComponent<Knockback>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (knockback.gettingKnockedBack) { return; }

        rb.MovePosition(rb.position + moveDir * (moveSpeed * Time.fixedDeltaTime));

        AdjustFacingDirection();
    }

    public void MoveTo(Vector2 targetPosition)
    {
        moveDir = targetPosition;
    }

    private void AdjustFacingDirection()
    {
        // Mevcut ölçeði al (0.8, 1, vs. neyse bozulmasýn diye)
        float currentScaleX = Mathf.Abs(transform.localScale.x);
        float currentScaleY = transform.localScale.y;
        float currentScaleZ = transform.localScale.z;

        // ---------------------------------------------------------
        // SENARYO 1: HAREKET SAÐA DOÐRU (x > 0)
        // ---------------------------------------------------------
        if (moveDir.x > 0)
        {
            if (originalFacingDirection == FacingDirection.Right)
            {
                // Resim zaten saða bakýyor -> Düz dur (+)
                transform.localScale = new Vector3(currentScaleX, currentScaleY, currentScaleZ);
            }
            else // Original Left
            {
                // Resim sola bakýyor -> Ters çevir (-) ki saða baksýn
                transform.localScale = new Vector3(-currentScaleX, currentScaleY, currentScaleZ);
            }
        }
        // ---------------------------------------------------------
        // SENARYO 2: HAREKET SOLA DOÐRU (x < 0)
        // ---------------------------------------------------------
        else if (moveDir.x < 0)
        {
            if (originalFacingDirection == FacingDirection.Right)
            {
                // Resim saða bakýyor -> Ters çevir (-) ki sola baksýn
                transform.localScale = new Vector3(-currentScaleX, currentScaleY, currentScaleZ);
            }
            else // Original Left
            {
                // Resim zaten sola bakýyor -> Düz dur (+)
                transform.localScale = new Vector3(currentScaleX, currentScaleY, currentScaleZ);
            }
        }
    }
}