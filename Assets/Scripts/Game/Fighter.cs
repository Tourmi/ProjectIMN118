using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class Fighter : MonoBehaviour
{
    private const float ESPILON = 5f;

    [SerializeField]
    private float speed = 700;
    [SerializeField]
    private float jumpSpeed = 1800;
    [SerializeField]
    private float dashSpeed = 1200;
    [SerializeField]
    private int dashDuration = 15;
    [SerializeField]
    public SpriteRenderer Sprite;
    [SerializeField]
    public Transform Opponent;

    [Header("Attacks")]
    [SerializeField]
    private Attack lightAttack;
    [SerializeField]
    private Attack mediumAttack;
    [SerializeField]
    private Attack heavyAttack;
    [SerializeField]
    private Attack specialAttack;

    [SerializeField]
    private Attack lightAirAttack;
    [SerializeField]
    private Attack mediumAirAttack;
    [SerializeField]
    private Attack heavyAirAttack;
    [SerializeField]
    private Attack specialAirAttack;

    [SerializeField]
    private Attack lightCrouchAttack;
    [SerializeField]
    private Attack mediumCrouchAttack;
    [SerializeField]
    private Attack heavyCrouchAttack;
    [SerializeField]
    private Attack specialCrouchAttack;

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

    private bool wasAirborne = false;

    private bool isLightAttacking;
    private bool isMediumAttacking;
    private bool isHeavyAttacking;
    private bool isSpecialAttacking;

    private bool attackLanded;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        //Attack ended event
        lightAttack.OnAttackEnd += HandleAttackEnd;
        mediumAttack.OnAttackEnd += HandleAttackEnd;
        heavyAttack.OnAttackEnd += HandleAttackEnd;
        specialAttack.OnAttackEnd += HandleAttackEnd;

        lightAirAttack.OnAttackEnd += HandleAttackEnd;
        mediumAirAttack.OnAttackEnd += HandleAttackEnd;
        heavyAirAttack.OnAttackEnd += HandleAttackEnd;
        specialAirAttack.OnAttackEnd += HandleAttackEnd;

        lightCrouchAttack.OnAttackEnd += HandleAttackEnd;
        mediumCrouchAttack.OnAttackEnd += HandleAttackEnd;
        heavyCrouchAttack.OnAttackEnd += HandleAttackEnd;
        specialCrouchAttack.OnAttackEnd += HandleAttackEnd;

        //Landed attack event
        lightAttack.OnAttackLanded += HandleAttackLanded;
        mediumAttack.OnAttackLanded += HandleAttackLanded;
        heavyAttack.OnAttackLanded += HandleAttackLanded;
        specialAttack.OnAttackLanded += HandleAttackLanded;

        lightAirAttack.OnAttackLanded += HandleAttackLanded;
        mediumAirAttack.OnAttackLanded += HandleAttackLanded;
        heavyAirAttack.OnAttackLanded += HandleAttackLanded;
        specialAirAttack.OnAttackLanded += HandleAttackLanded;

        lightCrouchAttack.OnAttackLanded += HandleAttackLanded;
        mediumCrouchAttack.OnAttackLanded += HandleAttackLanded;
        heavyCrouchAttack.OnAttackLanded += HandleAttackLanded;
        specialCrouchAttack.OnAttackLanded += HandleAttackLanded;
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
    public bool IsCrouchBlocking => IsBlocking && isCrouching;
    public bool IsStandingBlocking => IsBlocking && !isCrouching;
    public bool IsMovingAway => movement * EnemyDirection < 0;
    public bool IsAgainstWall => Physics2D.RaycastAll(transform.position, Vector2.left, -transform.localScale.x / 2 + ESPILON).Where(r => r.transform.CompareTag("Wall")).Any()
        || Physics2D.RaycastAll(transform.position, Vector2.right, transform.localScale.x / 2 + ESPILON).Where(r => r.transform.CompareTag("Wall")).Any();
    public int EnemyDirection => (Opponent.transform.position.x - transform.position.x) < 0 ? 1 : -1;

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
    private bool CanDash => (!isAttacking || attackLanded) && BlockStun <= 0 && HitStun <= 0 && !FightFinished && !isCrouching && !isDashing;

    private void FixedUpdate()
    {
        if (isCrouching && CanCrouch)
        {
            Sprite.transform.localScale = new Vector3(1f, 0.5f, 1f);
            Sprite.transform.localPosition = new Vector3(0f, -0.25f, 0f);
        }
        else
        {
            Sprite.transform.localScale = Vector3.one;
            Sprite.transform.localPosition = Vector3.zero;
        }

        if (isLightAttacking && CanAttack)
        {
            CancelAttacks();
            isAttacking = true;
            isLightAttacking = false;
            attackLanded = false;

            Attack attack = lightAttack;
            if (!IsGrounded) attack = lightAirAttack;
            else if (isCrouching && CanCrouch) attack = lightCrouchAttack;
            attack.StartAttack();
        }

        if (isMediumAttacking && CanAttack)
        {
            CancelAttacks();
            isAttacking = true;
            isMediumAttacking = false;
            attackLanded = false;

            Attack attack = mediumAttack;
            if (!IsGrounded) attack = mediumAirAttack;
            else if (isCrouching && CanCrouch) attack = mediumCrouchAttack;
            attack.StartAttack();
        }

        if (isHeavyAttacking && CanAttack)
        {
            CancelAttacks();
            isAttacking = true;
            isHeavyAttacking = false;
            attackLanded = false;

            Attack attack = heavyAttack;
            if (!IsGrounded) attack = heavyAirAttack;
            else if (isCrouching && CanCrouch) attack = heavyCrouchAttack;
            attack.StartAttack();
        }

        if (isSpecialAttacking && CanAttack)
        {
            CancelAttacks();
            isAttacking = true;
            isSpecialAttacking = false;
            attackLanded = false;

            Attack attack = specialAttack;
            if (!IsGrounded) attack = specialAirAttack;
            else if (isCrouching && CanCrouch) attack = specialCrouchAttack;
            attack.StartAttack();
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
            CancelAttacks();

            if (DashTimer <= 0)
            {
                isDashing = false;
            }
            else
            {
                rigidbody.velocity = new Vector2(dashMovement * dashSpeed, 0);
            }
        }

        if (wasAirborne && IsGrounded)
        {
            FighterLanded();
        }

        wasAirborne = !IsGrounded;

        HitStun--;
        BlockStun--;
        DashTimer--;
    }

    private void FighterLanded()
    {
        lightAirAttack.CancelAttack();
        mediumAirAttack.CancelAttack();
        heavyAirAttack.CancelAttack();
        specialAirAttack.CancelAttack();

        isAttacking = false;
        attackLanded = false;
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

        lightAirAttack.CancelAttack();
        mediumAirAttack.CancelAttack();
        heavyAirAttack.CancelAttack();
        specialAirAttack.CancelAttack();

        lightCrouchAttack.CancelAttack();
        mediumCrouchAttack.CancelAttack();
        heavyCrouchAttack.CancelAttack();
        specialCrouchAttack.CancelAttack();

        isAttacking = false;
        attackLanded = false;
    }
}
