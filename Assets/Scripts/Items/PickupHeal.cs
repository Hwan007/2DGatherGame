using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHeal : PickupItem
{
    [SerializeField] int healValue = 10;
    private HealthSystem _healtSystem;
    protected override void OnPickUp(GameObject receiver)
    {
        _healtSystem = receiver.GetComponent<HealthSystem>();
        _healtSystem.ChangeHealth(healValue);
    }
}
