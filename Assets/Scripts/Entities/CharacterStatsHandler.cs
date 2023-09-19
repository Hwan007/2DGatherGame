using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterStatsHandler : MonoBehaviour
{
    private const float MinAttackDelay = 0.03f;
    private const float MinAttackpower = 0.5f;
    private const float MinAttackSize = 0.4f;
    private const float MinAttackSpeed = 0.1f;

    private const float MinSpeed = 0.8f;

    private const int MinMaxHealth = 5;

    [SerializeField] private CharacterStats baseStats;

    public CharacterStats CurrentStats { get; private set; }
    public List<CharacterStats> statsModifiers = new List<CharacterStats>();

    private void Awake()
    {
        UpdateCharacterData();
    }

    private void UpdateCharacterData()
    {
        AttackSO attackSO = null;
        if (baseStats.attackSO != null)
        {
            attackSO = Instantiate(baseStats.attackSO);
        }

        CurrentStats = new CharacterStats() { attackSO = attackSO };
        // TODO
        // 추가적으로 계산을 할 예정
        UpdateStats((a, b) => b, baseStats);
        if (CurrentStats.attackSO != null)
        {
            CurrentStats.attackSO.target = baseStats.attackSO.target;
        }

        foreach (CharacterStats modifier in statsModifiers.OrderBy(x => x.statsChangeType))
        {
            if (modifier.statsChangeType == eStatsChangeType.Override)
            {
                UpdateStats((x, y) => y, modifier);
            }
            else if (modifier.statsChangeType == eStatsChangeType.Add)
            {
                UpdateStats((x, y) => x + y, modifier);
            }
            else if (modifier.statsChangeType == eStatsChangeType.Multiple)
            {
                UpdateStats((x, y) => x * y, modifier);
            }
        }
        // ------------------------
        //CurrentStats.statsChangeType = baseStats.statsChangeType;
        //CurrentStats.maxHealth = baseStats.maxHealth;
        //CurrentStats.speed = baseStats.speed;
    }

    private void UpdateStats(Func<float, float, float> operation, CharacterStats newModifier)
    {
        CurrentStats.maxHealth = (int)operation(CurrentStats.maxHealth, newModifier.maxHealth);
        CurrentStats.speed = operation(CurrentStats.speed, newModifier.speed);

        UpdateAttackStats(operation, CurrentStats.attackSO, newModifier.attackSO);

        if (CurrentStats.attackSO.GetType() != newModifier.attackSO.GetType())
        {
            return;
        }

        switch (CurrentStats.attackSO)
        {
            case RangedAttackData:
                ApplyRangedStats(operation, newModifier);
                break;
        }
    }

    private void ApplyRangedStats(Func<float, float, float> operation, CharacterStats newModifier)
    {
        RangedAttackData currentRangedAttacks = CurrentStats.attackSO as RangedAttackData;

        if (!(newModifier.attackSO is RangedAttackData))
        {
            return;
        }

        RangedAttackData rangedAttacksModifier = newModifier.attackSO as RangedAttackData;
        currentRangedAttacks.multipleProjectilesAngle = operation(currentRangedAttacks.multipleProjectilesAngle, rangedAttacksModifier.multipleProjectilesAngle);
        currentRangedAttacks.spread = operation(currentRangedAttacks.spread, rangedAttacksModifier.spread);
        currentRangedAttacks.duration = operation(currentRangedAttacks.duration, rangedAttacksModifier.duration);
        currentRangedAttacks.numberofProjectilesPerShot = Mathf.CeilToInt(operation(currentRangedAttacks.numberofProjectilesPerShot, rangedAttacksModifier.numberofProjectilesPerShot));
        currentRangedAttacks.projectileColor = UpdateColor(operation, currentRangedAttacks.projectileColor, rangedAttacksModifier.projectileColor);
    }

    private Color UpdateColor(Func<float, float, float> operation, Color currentColor, Color newColor)
    {
        return new Color(
            operation(currentColor.r, newColor.r),
            operation(currentColor.g, newColor.g),
            operation(currentColor.b, newColor.b),
            operation(currentColor.a, newColor.a));
    }

    private void UpdateAttackStats(Func<float, float, float> operation, AttackSO currentAttack, AttackSO newAttack)
    {
        if (currentAttack == null || newAttack == null || currentAttack.GetType() != newAttack.GetType())
        {
            return;
        }

        currentAttack.delay = operation(currentAttack.delay, newAttack.delay);
        currentAttack.power = operation(currentAttack.power, newAttack.power);
        currentAttack.size = operation(currentAttack.size, newAttack.size);
        currentAttack.speed = operation(currentAttack.speed, newAttack.speed);
    }

    public void AddStatModifier(CharacterStats statModifier)
    {
        statsModifiers.Add(statModifier);
        UpdateCharacterData();
    }

    public void RemoveStatModifier(CharacterStats statModifier)
    {
        statsModifiers.Remove(statModifier);
        UpdateCharacterData();
    }

    private void LimitStats(ref float stat, float minVal)
    {
        stat = Mathf.Max(stat, minVal);
    }

    private void LimitAllStats()
    {
        if (CurrentStats == null || CurrentStats.attackSO == null)
        {
            return;
        }

        LimitStats(ref CurrentStats.attackSO.delay, MinAttackDelay);
        LimitStats(ref CurrentStats.attackSO.power, MinAttackpower);
        LimitStats(ref CurrentStats.attackSO.size, MinAttackpower);
        LimitStats(ref CurrentStats.attackSO.speed, MinAttackSpeed);
        LimitStats(ref CurrentStats.speed, MinSpeed);
        CurrentStats.maxHealth = Mathf.Max(CurrentStats.maxHealth, MinMaxHealth);
    }
}
