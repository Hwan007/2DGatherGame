using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eStatsChangeType
{
    Add,
    Multiple,
    Override,
}
[Serializable]
public class CharacterStats

{
    public eStatsChangeType statsChangeType;
    [SerializeField, Range(1, 100)] public int maxHealth;
    [SerializeField, Range(1f, 100f)] public int speed;

    // ���� ������
    public AttackSO attackSO;

}
