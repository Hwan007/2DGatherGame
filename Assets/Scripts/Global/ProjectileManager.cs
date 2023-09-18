using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem _impactParticleSystem;

    public static ProjectileManager Instance;

    private ObjectPool objectPool;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        objectPool = GetComponent<ObjectPool>();
    }

    public void ShootBullet(Vector2 startPosition, Vector2 direction, RangedAttackData attackData)
    {
        GameObject obj = objectPool.SpawnFromPool(attackData.bulletNameTag);

        obj.transform.position = startPosition;
        // TODO : 날라가게 만들어야 한다.
        RangedAttackController attackController  = obj.GetComponent<RangedAttackController>();
        attackController.InitializeAttack(direction, attackData, this);
        
        obj.SetActive(true);
    }

    public void CreateImpactParticlesAtPosition(Vector3 position, RangedAttackData attackData)
    {
        _impactParticleSystem.transform.position = position;
        ParticleSystem.EmissionModule em = _impactParticleSystem.emission;
        em.SetBurst(0, new ParticleSystem.Burst(0, Mathf.Ceil(attackData.size * 5)));
        ParticleSystem.MainModule mainModule = _impactParticleSystem.main;
        mainModule.startSpeedMultiplier = attackData.size * 10f;
        _impactParticleSystem.Play();
    }
}
