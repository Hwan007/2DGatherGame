using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownShooting : MonoBehaviour
{
    private TopDownCharacterController _controller;

    [SerializeField] private Transform projectileSpawnPostion;

    private void Awake()
    {
        _controller = GetComponent<TopDownCharacterController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        _controller.OnAttackEvent += OnShoot;
        _controller.OnLookEvent += Look;
    }

    private void OnShoot()
    {
        CreateProjectile();
    }

    private void Look(Vector2 newAimDirection)
    {

    }

    private void CreateProjectile()
    {
        Debug.Log("CreateProjectile");
    }
}
