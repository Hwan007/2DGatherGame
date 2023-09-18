using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustParticleControl : MonoBehaviour
{
    [SerializeField] private ParticleSystem dustParticleSystem;
    [SerializeField] private bool createDustOnWalk = true;

    public void CreateDustParticles()
    {
        if (createDustOnWalk)
        {
            dustParticleSystem.Stop();
            dustParticleSystem.Play();
        }
    }
}
