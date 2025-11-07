using System;
using System.Collections.Generic;
using Runtime.Constant;
using Runtime.Stat;
using UnityEngine;

namespace Runtime.Skill
{
    [CreateAssetMenu(fileName = "New Skill Stat Buff", menuName = "Game SO/Skill Stat Buff")]
    public class StatBuff : BaseBuff
    {
        public StatBuffConfig[] buffs;
        private List<StatModifier> _statModifiers;
        
        public override void Apply(WeaponStat stats)
        {
            _statModifiers = new();
            foreach (var item in buffs)
            {
                var statModifier = new StatModifier(item.statType, item.value, item.modifierType);
                stats.AddModifier(item.statType, statModifier);
                _statModifiers.Add(statModifier);
            }
        }

        public override void Remove(WeaponStat stats)
        {
            foreach (var item in _statModifiers)
            {
                stats.RemoveModifier(item.statType, item);
            }
        }
    }

    [Serializable]
    public struct StatBuffConfig
    {
        public StatType statType;
        public float value;
        public ModifierType modifierType;
    }
}