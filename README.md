# 2DGatherGame
![ezgif com-video-to-gif](https://github.com/Hwan007/2DGatherGame/assets/96556920/b74a2685-aad9-4847-8915-10f8119d8bda)

### 작성하면서 고려사항

</br>
1. TopDownCharacterController.cs의 event에 등록해서 자동적으로 구현

TopDownCameraMovement.cs => TopDownCharacterController의 event에 등록
```
private void Awake()
{
  _controller = _player?.GetComponent<TopDownCharacterController>();
}
private void Start()
{
  _controller.OnMoveEvent += Move;
  _controller.OnAimEvent += Aim;
  _controller.OnLookEvent += Look;
}
```

TopDownCharacterController.cs => Aim event 추가
```
public event Func<bool,bool> OnAimEvent;
public void CallAimEvent(bool input)
{
  OnAimEvent?.Invoke(input);
}
```

</br>
2. 부드러운 화면 이동과 Aim 시에 이동 속도가 느려지는 것을 구현

TopDownCameraMovement.cs => 부드러운 화면 이동
```
private void FixedUpdate()
{
  ApplyMovement(_movementDirection);
}
public void Look(Vector2 direction)
{
  _offsetPosition = direction*4;
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
```

PlayerInputController.cs => 속도 느려짐
```
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
```


