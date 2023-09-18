using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class TopDownCharacterController : MonoBehaviour
{
    // event 외부에서 호출하지 못하도록 막는다.
    public event Action<Vector2> OnMoveEvent;
    public event Action<Vector2> OnLookEvent;
    public event Action<AttackSO, bool> OnAttackEvent;
    public event Func<bool,bool> OnAimEvent;

    private float _timeSinceLastAttack = float.MaxValue;
    protected bool IsAttacking { get; set; }
    protected bool IsAim { get; set; }

    protected CharacterStatsHandler Stats { get; private set; }

    protected virtual void Awake()
    {
        Stats = GetComponent<CharacterStatsHandler>();
    }
    protected virtual void Update()
    {
        HandleAttackDelay();
    }

    private void HandleAttackDelay()
    {
        if (Stats.CurrentStats.attackSO == null)
            return;

        if (_timeSinceLastAttack <= Stats.CurrentStats.attackSO.delay)
        {
            _timeSinceLastAttack += Time.deltaTime;
        }
        else if (IsAttacking)
        {
            _timeSinceLastAttack = 0;
            CallAttackEvent(Stats.CurrentStats.attackSO, IsAim);
        }
    }
    public void CallMoveEvent(Vector2 direction)
    {
        OnMoveEvent?.Invoke(direction);
    }
    public void CallLookEvent(Vector2 direction)
    {
        OnLookEvent?.Invoke(direction);
    }
    public void CallAttackEvent(AttackSO attackSO, bool isAim)
    {
        OnAttackEvent?.Invoke(attackSO, isAim);
    }
    public void CallAimEvent(bool input)
    {
        OnAimEvent?.Invoke(input);
    }
}