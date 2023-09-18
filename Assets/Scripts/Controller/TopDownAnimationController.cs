using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownAnimationController : TopDownAnimations
{
    // 문자열 비교가 비싸므로, 숫자값으로 비교를 하자.
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int IsHit = Animator.StringToHash("IsHit");

    private HealthSystem _healthSystem;
    protected override void Awake()
    {
        base.Awake();
        _healthSystem = GetComponent<HealthSystem>();
    }

    private void Start()
    {
        controller.OnAttackEvent += Attacking;
        controller.OnMoveEvent += Move;

        if (_healthSystem != null )
        {
            _healthSystem.OnDamage += Hit;
            _healthSystem.OnInvincibilityEnd += InvincibilityEnd;
        }
    }

    private void Move(Vector2 vector)
    {
        animator.SetBool(IsWalking, vector.magnitude > .5f);
    }

    private void Attacking(AttackSO sO, bool arg2)
    {
        animator.SetTrigger(Attack);
    }

    private void Hit()
    {
        //Debug.Log("Hit!");
        animator.SetBool(IsHit, true);
    }

    private void InvincibilityEnd()
    {
        animator.SetBool(IsHit, false);
    }
}
