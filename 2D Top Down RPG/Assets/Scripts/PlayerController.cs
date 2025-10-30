using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
public bool FacingLeft { get { return facingLeft; }  set { facingLeft = value; } } 

[SerializeField] private float moveSpeed = 1f;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAanimator;
    private SpriteRenderer mySpriteRender;
    private bool facingLeft = false;

private void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAanimator = GetComponent<Animator>();
        mySpriteRender = GetComponent<SpriteRenderer>(); 
    }

    private void OnEnable()
    {
        playerControls.Enable();
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
}