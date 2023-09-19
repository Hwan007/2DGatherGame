using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCameraMovement : MonoBehaviour
{
    private TopDownCharacterController _controller;
    [SerializeField] private GameObject _player;
    private Vector2 _movementDirection;
    private Vector3 _targetPosition;
    private Vector2 _offsetPosition = Vector2.zero;
    private bool _isAim = false;

    private void Awake()
    {
        _controller = _player.GetComponent<TopDownCharacterController>();
    }

    private void Start()
    {
        _controller.OnMoveEvent += Move;
        _controller.OnAimEvent += Aim;
        _controller.OnLookEvent += Look;
    }
    private void FixedUpdate()
    {
        ApplyMovement(_movementDirection);
    }
    public void Look(Vector2 direction)
    {
        _offsetPosition = direction*4;
    }
    public bool Aim(bool input)
    {
        _isAim = input;
        return input;
    }
    public void Move(Vector2 direction)
    {
        _movementDirection = direction;
    }
    private void ApplyMovement(Vector2 direction)
    {
        if (_isAim)
            _targetPosition = (Vector2)_player.transform.position + _offsetPosition;
        else
            _targetPosition = (Vector2)_player.transform.position;

        if ((_targetPosition - transform.position).magnitude >= 0.1f)
        {
            float x = Mathf.Lerp(transform.position.x, _targetPosition.x, 0.3f);
            float y = Mathf.Lerp(transform.position.y, _targetPosition.y, 0.3f);
            transform.position = new Vector3(x, y, -10);
        }
        else
        {
            transform.position = new Vector3(_targetPosition.x, _targetPosition.y, -10);
        }
    }
}
