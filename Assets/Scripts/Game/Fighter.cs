using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Fighter : MonoBehaviour
{
    private const float ESPILON = 1f;


    [SerializeField]
    private float speed = 700;
    [SerializeField]
    private float jumpSpeed = 1800;

    private Rigidbody2D rigidbody;
    private bool isGrounded = true;

    private bool isJumping;
    private int movement;
    private bool isCrouching;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnJump(InputValue value) => isJumping = value.isPressed;

    private void OnMove(InputValue value)
    {
        float movementValue = value.Get<float>();
        if (movementValue > 0.5)
        {
            movement = 1;
        }
        else if (movementValue < -0.5)
        {
            movement = -1;
        }
        else
        {
            movement = 0;
        }
    }

    private void OnCrouch(InputValue value) => isCrouching = value.isPressed;

    private void FixedUpdate()
    {
        var result = Physics2D.RaycastAll(transform.position, Vector2.down, transform.localScale.y / 2 + ESPILON);
        isGrounded = result.Where(r => r.transform.CompareTag("Ground")).Any();

        if (isGrounded)
        {
            rigidbody.velocity = new Vector2(movement * speed, rigidbody.velocity.y);
        }

        if (isJumping && isGrounded)
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpSpeed);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
