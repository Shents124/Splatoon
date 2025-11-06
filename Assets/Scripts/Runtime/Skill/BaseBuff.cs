using UnityEngine;

namespace Runtime.Skill
{
    public class BaseBuff : ScriptableObject, IBuff
    {
        public int id;
        public int upgradeId;
        
        public virtual void Apply(WeaponStat stats)
        {
        }

        public virtual void Remove(WeaponStat stats)
        {
        }

        public virtual void UpdateBuff(WeaponStat stats)
        {
        }
    }
}