using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : TopDownCharacterController
{
    private Camera _camera;
    private Vector2 _moveInput;
    protected override void Awake()
    {
        base.Awake();
        _camera = Camera.main;
    }

    public void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>().normalized;
        if (IsAim)
        {
            CallMoveEvent(_moveInput / 2);
        }
        else
            CallMoveEvent(_moveInput);
    }

    public void OnLook(InputValue value)
    {
        Vector2 newAim = value.Get<Vector2>();
        Vector2 worldPos = _camera.ScreenToWorldPoint(newAim);
        newAim = (worldPos - (Vector2)transform.position).normalized;

        if (newAim.magnitude >= .9f)
        {
            CallLookEvent(newAim);
        }
    }

    public void OnFire(InputValue value)
    {
        IsAttacking = value.isPressed;
    }

    public void OnAim(InputValue value)
    {
        IsAim = value.isPressed;
        CallAimEvent(IsAim);
        if (IsAim)
        {
            CallMoveEvent(_moveInput/2);
        }
        else
            CallMoveEvent(_moveInput);
    }
}
