using System;
using Runtime.PubSub;
using Runtime.PubSub.CommonMessage;
using Runtime.Stat;

namespace Runtime.Skill
{
    [Serializable]
    public class SpikeShieldSkill : BaseSkill
    {
        public float shieldPercent;
        public float attackPercent;

        public override void Apply(WeaponStat stats)
        {
            base.Apply(stats);
            
            var msg = new SpawnSpikeShieldMessage()
            {
                shield = stats.health.value * shieldPercent,
                damage = attackPercent * weaponStat.attack.value
            };
            WorldMessenger.Pub(msg);
        }
    }
}