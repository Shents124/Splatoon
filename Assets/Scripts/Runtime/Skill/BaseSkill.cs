using System;
using Runtime.Stat;
using ZBase.Foundation.PubSub;

namespace Runtime.Skill
{
    [Serializable]
    public abstract class BaseSkill
    {
        protected ISubscription subscription;
        protected WeaponStat weaponStat;

        public virtual void Init()
        {
            
        }
        
        public virtual void Apply(WeaponStat stats)
        {
            weaponStat = stats;
        }

        public virtual void Remove(WeaponStat stats)
        {
            
        }

        public void CleanUp()
        {
            subscription?.Unsubscribe();
        }
    }
}