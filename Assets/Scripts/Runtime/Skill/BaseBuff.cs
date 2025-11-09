using System;
using Runtime.Constant;
using Runtime.Stat;
using UnityEngine;

namespace Runtime.Skill
{
    public class BaseBuff : ScriptableObject, IBuff, IEquatable<BaseBuff>
    {
        public int rootId;
        public int id;
        public int upgradeId;
        
        public string skillName;
        [TextArea]
        public string description;
        public BuffRarity rarity;

        public virtual void Init()
        {
            
        }
        
        public virtual void Apply(WeaponStat stats) { }

        public virtual void Remove(WeaponStat stats) { }

        public virtual void UpdateBuff(WeaponStat stats) { }

        public bool Equals(BaseBuff other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && rootId == other.rootId && id == other.id;
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((BaseBuff)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), rootId, id);
        }
    }
}