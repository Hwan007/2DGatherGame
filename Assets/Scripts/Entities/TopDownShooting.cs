using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class TopDownShooting : MonoBehaviour
{
    private TopDownCharacterController _controller;
    private ProjectileManager _projectileManager;

    [SerializeField] private Transform projectileSpawnPostion;
    private Vector2 _lookDirection;
    //private CharacterStatsHandler _Stats;

    public AudioClip shootingClip;
    private void Awake()
    {
        _controller = GetComponent<TopDownCharacterController>();
        //_Stats = GetComponent<CharacterStatsHandler>();
    }
    // Start is called before the first frame update
    void Start()
    {
        _projectileManager = ProjectileManager.Instance;
        _controller.OnAttackEvent += OnShoot;
        _controller.OnLookEvent += Look;
    }

    private void OnShoot(AttackSO attackSO, bool isAim)
    {
        RangedAttackData rangedAttackData = attackSO as RangedAttackData;
        float projectilesAngleSpace = rangedAttackData.multipleProjectilesAngle;
        int numberOfProjectilePerShot = rangedAttackData.numberofProjectilesPerShot;

        float minAngle = -(numberOfProjectilePerShot / 2f) * projectilesAngleSpace + 0.5f * rangedAttackData.multipleProjectilesAngle;

        for (int i = 0; i < numberOfProjectilePerShot; ++i)
        {
            float angle = minAngle + projectilesAngleSpace * i;
            float randomSpread;
            if (isAim)
                randomSpread = Random.Range(-rangedAttackData.spread/2, +rangedAttackData.spread/2);
            else
                randomSpread = Random.Range(-rangedAttackData.spread, +rangedAttackData.spread);
            angle += randomSpread;
            CreateProjectile(rangedAttackData, angle);
        }
    }

    public void Look(Vector2 newAimDirection)
    {
        _lookDirection = newAimDirection;
    }

    private void CreateProjectile(RangedAttackData rangedAttackData, float angle)
    {
        _projectileManager.ShootBullet(
            projectileSpawnPostion.position,
            RotateVector2(_lookDirection, angle),
            rangedAttackData
            );
        //Debug.Log("Attack");
        if (shootingClip)
            SoundManager.PlayClip(shootingClip);
    }

    private static Vector2 RotateVector2(Vector2 v, float degree)
    {
        return Quaternion.Euler(0, 0, degree) * v;
    }
}
