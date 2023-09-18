using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatsHandler : MonoBehaviour
{
    [SerializeField] private CharacterStats _baseData;

    public CharacterStats CurrentStats { get; private set; }
    public List<CharacterStats> dataModifiers  = new List<CharacterStats>();

    private void Awake()
    {
        UpdateCharacterData();
    }

    private void UpdateCharacterData()
    {
        AttackSO attackSO = null;
        if (_baseData.attackSO != null)
        {
            attackSO = Instantiate(_baseData.attackSO);
        }

        CurrentStats = new CharacterStats() { attackSO = attackSO };
        // TODO
        // 추가적으로 계산을 할 예정
        CurrentStats.statsChangeType = _baseData.statsChangeType;
        CurrentStats.maxHealth = _baseData.maxHealth;
        CurrentStats.speed = _baseData.speed;

    }
}
