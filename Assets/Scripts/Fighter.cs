using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        var result = Physics2D.RaycastAll(transform.position, Vector2.down, transform.localScale.y / 2 + ESPILON);
        isGrounded = result.Where(r => r.transform.CompareTag("Ground")).Any();

        if (Input.GetAxisRaw("Horizontal") > 0.5)
        {
            rigidbody.velocity = new Vector2(speed, rigidbody.velocity.y);
        }
        else if (Input.GetAxisRaw("Horizontal") < -0.5)
        {
            rigidbody.velocity = new Vector2(-speed, rigidbody.velocity.y);
        } else
        {
            rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
        }

        if (Input.GetAxisRaw("Vertical") > 0.5 && isGrounded)
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
