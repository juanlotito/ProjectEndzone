using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimatorController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void Walk(bool isWalking)
    {
        animator.SetBool("IsWalking", isWalking);
    }

    public void Sprint(bool isSprinting)
    {
        animator.SetBool("IsSprinting", isSprinting);
    }

    public void HitMelee(bool isHittingMelee)
    {
        animator.SetBool("IsHittingMelee", isHittingMelee);
    }

    public void Dead (bool isDead)
    {
        animator.SetBool("IsDead", isDead);
    }

    public void Detection()
    {
        animator.SetTrigger("Detection");
    }

    internal void AttackAmbush(bool ambush)
    {
        animator.SetBool("Ambush", ambush);
    }
}
