using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public bool FacingLeft { get { return facingLeft; } set { facingLeft = value; } }
    public static PlayerController Instance;

    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float dashSpeed = 4f;
    [SerializeField] private TrailRenderer myTrailRenderer;

    // --- YENÝ SES DEÐÝÞKENÝ ---
    [Tooltip("Dash AudioSource'u")]
    [SerializeField] private AudioSource dashSound; // <-- YENÝ
    // ----------------------------

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAanimator;
    private SpriteRenderer mySpriteRender;

    private bool facingLeft = false;
    private bool isDashing = false;

    private void Awake()
    {
        Instance = this;
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAanimator = GetComponent<Animator>();
        mySpriteRender = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        playerControls.Combat.Dash.performed += _ => Dash();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable() // <-- EKSÝK METOT (Eklemeniz iyi bir pratiktir)
    {
        playerControls.Disable();
    }

    private void PlayerInput()
    {
        movement = playerControls.Player.Move.ReadValue<Vector2>();
        myAanimator.SetFloat("moveX", movement.x);
        myAanimator.SetFloat("moveY", movement.y);
    }

    private void Move()
    {
        Vector2 newPosition = rb.position + movement * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }

    private void Update()
    {
        PlayerInput();
    }
    private void FixedUpdate()
    {
        AdjustPlayerFacingDirection();
        Move();
    }

    private void AdjustPlayerFacingDirection()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if (mousePos.x < playerScreenPoint.x)
        {
            mySpriteRender.flipX = true;
            FacingLeft = true;
        }
        else
        {
            mySpriteRender.flipX = false;
            FacingLeft = false;
        }

    }
    private void Dash()
    {
        if (!isDashing)
        {
            isDashing = true;
            moveSpeed *= dashSpeed;
            myTrailRenderer.emitting = true;

            // --- YENÝ SES ÇALMA KODU ---
            if (dashSound != null)
            {
                dashSound.Play(); // <-- YENÝ
            }
            // ---------------------------

            StartCoroutine(EndDashRoutine());
        }
    }


    private IEnumerator EndDashRoutine()
    {
        float dashTime = .2f;
        float dashCD = .25f;
        yield return new WaitForSeconds(dashTime);
        moveSpeed /= dashSpeed;
        myTrailRenderer.emitting = false;
        yield return new WaitForSeconds(dashCD);
        isDashing = false;
    }
}