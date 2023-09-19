using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupStatModifier : PickupItem
{
    [SerializeField] private List<CharacterStats> statsModifier;
    protected override void OnPickUp(GameObject receiver)
    {
        CharacterStatsHandler statsHandler = receiver.GetComponent<CharacterStatsHandler>();
        foreach (CharacterStats stat in statsModifier)
        {
            statsHandler.AddStatModifier(stat);
        }
    }
}
