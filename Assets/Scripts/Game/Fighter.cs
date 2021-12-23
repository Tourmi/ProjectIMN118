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
    [SerializeField]
    private float dashSpeed = 1200;
    [SerializeField]
    private int dashDuration = 15;
    [SerializeField]
    private Attack lightAttack;
    [SerializeField]
    private Attack mediumAttack;
    [SerializeField]
    private Attack heavyAttack;
    [SerializeField]
    private Attack specialAttack;
    [SerializeField]
    private Transform Sprite;

    public float MaxHealth = 3000;
    public float CurrentHealth = 3000;

    private new Rigidbody2D rigidbody;

    public bool FightStarted = false;
    public bool FightFinished = false;

    private bool isJumping;
    private int movement;
    private int dashMovement;
    private bool isCrouching;
    private bool isDashing;

    private bool isLightAttacking;
    private bool isMediumAttacking;
    private bool isHeavyAttacking;
    private bool isSpecialAttacking;

    private bool attackLanded;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        lightAttack.OnAttackEnd += HandleAttackEnd;
        mediumAttack.OnAttackEnd += HandleAttackEnd;
        heavyAttack.OnAttackEnd += HandleAttackEnd;
        specialAttack.OnAttackEnd += HandleAttackEnd;

        lightAttack.OnAttackLanded += HandleAttackLanded;
        mediumAttack.OnAttackLanded += HandleAttackLanded;
        heavyAttack.OnAttackLanded += HandleAttackLanded;
        specialAttack.OnAttackLanded += HandleAttackLanded;
    }

    public void Initialize(Vector3 initialPosition)
    {
        isJumping = false;
        movement = 0;
        isCrouching = false;
        FightStarted = false;
        FightFinished = false;

        isLightAttacking = false;
        isMediumAttacking = false;
        isHeavyAttacking = false;
        isSpecialAttacking = false;
        CurrentHealth = MaxHealth;

        transform.localPosition = initialPosition;
    }

    public int BlockStun { get; set; }
    public int HitStun { get; set; }
    public int DashTimer { get; set; }

    public bool IsBlocking => IsGrounded && movement == -1 && !isAttacking && HitStun <= 0;

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

    private void OnDashLeft()
    {
        if (CanDash)
        {
            dashMovement = -1;
            isDashing = true;
            DashTimer = dashDuration;
        }
    }
    private void OnDashRight()
    {
        if (CanDash)
        {
            dashMovement = 1;
            isDashing = true;
            DashTimer = dashDuration;
        }
    }

    private void OnLightAttack() => isLightAttacking = true;
    private void OnMediumAttack() => isMediumAttacking = true;
    private void OnHeavyAttack() => isHeavyAttacking = true && CurrentHealth <= MaxHealth / 3f * 2f;
    private void OnSpecialAttack() => isSpecialAttacking = true && CurrentHealth <= MaxHealth / 3f;

    private bool isAttacking;

    private bool IsGrounded => Physics2D.RaycastAll(transform.position, Vector2.down, transform.localScale.y / 2 + ESPILON).Where(r => r.transform.CompareTag("Ground")).Any();
    private bool CanCrouch => IsGrounded && !FightFinished && HitStun <= 0 && !isDashing;
    private bool CanAttack => (!isAttacking || attackLanded) && !FightFinished && FightStarted && BlockStun <= 0 && HitStun <= 0 && !isDashing;
    private bool CanMove => IsGrounded && !isAttacking && !isCrouching && !FightFinished && BlockStun <= 0 && HitStun <= 0 && !isDashing;
    private bool CanJump => IsGrounded && !isAttacking && !FightFinished && BlockStun <= 0 && HitStun <= 0 && !isDashing;
    private bool CanDash => !isAttacking && BlockStun <= 0 && HitStun <= 0 && !FightFinished && !isCrouching && !isDashing;

    private void FixedUpdate()
    {
        if (isCrouching && CanCrouch)
        {
            Sprite.localScale = new Vector3(1f, 0.5f, 1f);
            Sprite.localPosition = new Vector3(0f, -0.25f, 0f);
        }
        else
        {
            Sprite.localScale = Vector3.one;
            Sprite.localPosition = Vector3.zero;
        }

        if (isLightAttacking && CanAttack)
        {
            CancelAttacks();
            isAttacking = true;
            isLightAttacking = false;
            attackLanded = false;
            lightAttack.StartAttack();
        }

        if (isMediumAttacking && CanAttack)
        {
            CancelAttacks();
            isAttacking = true;
            isMediumAttacking = false;
            attackLanded = false;
            mediumAttack.StartAttack();
        }

        if (isHeavyAttacking && CanAttack)
        {
            CancelAttacks();
            isAttacking = true;
            isHeavyAttacking = false;
            attackLanded = false;
            heavyAttack.StartAttack();
        }

        if (isSpecialAttacking && CanAttack)
        {
            CancelAttacks();
            isAttacking = true;
            isSpecialAttacking = false;
            attackLanded = false;
            specialAttack.StartAttack();
        }

        if (CanMove)
        {
            rigidbody.velocity = new Vector2(movement * speed, rigidbody.velocity.y);
        }

        if (isJumping && CanJump)
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpSpeed);
        }

        if (isDashing)
        {
            if (DashTimer <= 0)
            {
                isDashing = false;
            }
            else
            {
                rigidbody.velocity = new Vector2(dashMovement * dashSpeed, 0);
            }
        }

        HitStun--;
        BlockStun--;
        DashTimer--;
    }

    private void HandleAttackEnd()
    {
        isAttacking = false;
    }

    private void HandleAttackLanded()
    {
        attackLanded = true;
    }

    private void CancelAttacks()
    {
        lightAttack.CancelAttack();
        mediumAttack.CancelAttack();
        heavyAttack.CancelAttack();
        specialAttack.CancelAttack();
    }
}
