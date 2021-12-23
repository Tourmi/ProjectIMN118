using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public event Action OnAttackEnd;
    public event Action OnAttackLanded;

    public Rigidbody2D Owner;
    public GameObject HitboxPreview;
    public GameObject HitboxVisual;
    public GameObject HitboxPostVisual;
    public int Damage;
    public int StartFrames;
    public int ActiveFrames;
    public int RecoveryFrames;
    public int HitStun;
    public int BlockStun;

    private int currentFrame = -1;

    private bool hitboxActive = false;

    private void FixedUpdate()
    {
        if (currentFrame >= 0)
        {
            if (currentFrame == 0)
            {
                HitboxPreview.SetActive(true);
            }
            else if (currentFrame == StartFrames)
            {
                HitboxPreview.SetActive(false);
                HitboxVisual.SetActive(true);
                hitboxActive = true;
            }
            else if (currentFrame == StartFrames + ActiveFrames)
            {
                HitboxVisual.SetActive(false);
                HitboxPostVisual.SetActive(true);
                hitboxActive = false;
            }
            else if (currentFrame >= StartFrames + ActiveFrames + RecoveryFrames)
            {
                OnAttackEnd?.Invoke();
                HitboxPostVisual.SetActive(false);
                currentFrame = -1;
                return;
            }

            currentFrame++;
        }
    }

    public void StartAttack()
    {
        currentFrame = 0;
    }

    public void CancelAttack()
    {
        currentFrame = -1;
        HitboxPostVisual.SetActive(false);
        HitboxVisual.SetActive(false);
        HitboxPreview.SetActive(false);
        hitboxActive = false;
    }

    //Attack landed
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.gameObject != Owner.gameObject && hitboxActive)
        {
            bool isToTheRight = transform.position.x - collision.transform.position.x < 0;

            Fighter otherFighter = collision.GetComponent<Fighter>();
            Rigidbody2D otherRigidBody = collision.GetComponent<Rigidbody2D>();
            if (otherFighter.IsBlocking)
            {
                otherFighter.CurrentHealth -= Damage / 4;
                otherFighter.BlockStun = BlockStun;
                otherRigidBody.velocity = new Vector2(isToTheRight ? Damage * 10 : -Damage * 10, otherRigidBody.velocity.y);
                Owner.velocity = new Vector2(isToTheRight ? -Damage * 10 : Damage * 10, Owner.velocity.y);
            }
            else
            {
                otherFighter.CurrentHealth -= Damage;
                otherFighter.HitStun = HitStun;
                otherRigidBody.velocity = new Vector2(isToTheRight ? Damage * 20 : -Damage * 20, otherRigidBody.velocity.y);
                Owner.velocity = new Vector2(isToTheRight ? -Damage * 10 : Damage * 10, Owner.velocity.y);
            }
            hitboxActive = false;

            OnAttackLanded?.Invoke();
        }
    }
}
