using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimatorController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void Walk(bool isWalking)
    {
        if (isWalking) animator.SetBool("IsWalking", true);
        else animator.SetBool("IsWalking", false);
    }

    public void Sprint(bool isSprinting)
    {
        if (isSprinting) animator.SetBool("IsSprinting", true);
        else animator.SetBool("IsSprinting", false);
    }

    public void HitMelee(bool isHittingMelee)
    {
        if (isHittingMelee) animator.SetBool("IsHittingMelee", true);
        else animator.SetBool("IsHittingMelee", false);
    }

    public void ZombieWalk(bool isMoving)
    {
        if (isMoving) animator.SetBool("IsMoving", isMoving);
        else animator.SetBool("IsMoving", isMoving);
    }

    public void ZombieChasing(bool isRunning)
    {
        if (isRunning) animator.SetBool("IsRunning", isRunning);
        else animator.SetBool("IsRunning", isRunning);
    }

    public void ZombieHitMelee(bool isAttacking)
    {
        if (isAttacking) animator.SetBool("IsAttacking", isAttacking);
        else animator.SetBool("IsAttacking", isAttacking);
    }

    public void ZombieDead (bool isDead)
    {
        if (isDead) animator.SetBool("IsDead", isDead);
        else animator.SetBool("IsDead", isDead);
    }

    public void ZombieDetection()
    {
        animator.SetTrigger("Detection");
    }

    internal void ZombieAttackAmbush(bool ambush)
    {
        animator.SetBool("Ambush", ambush);
    }
}