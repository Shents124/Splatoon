using System;
using Runtime.PubSub;
using Runtime.PubSub.CommonMessage;
using Runtime.Stat;

namespace Runtime.Skill
{
    [Serializable]
    public class CombatSupportSkill : BaseSkill
    {
        public int numberDrone;
        public float damagePercent;
        public float fireRate;

        public override void Apply(WeaponStat stats)
        {
            base.Apply(stats);
            
            WorldMessenger.Pub(new SpawnDroneMessage()
            {
                numberDrone = numberDrone,
                damage = damagePercent * stats.attack.value,
                fireRate = fireRate
            });
        }
    }
}