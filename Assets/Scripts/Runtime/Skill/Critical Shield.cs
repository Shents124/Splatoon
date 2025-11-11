using System;
using Runtime.Manager;
using Runtime.PubSub;
using Runtime.PubSub.CommonMessage;
using Runtime.Stat;
using Object = UnityEngine.Object;

namespace Runtime.Skill
{
    [Serializable]
    public class CriticalShield : BaseSkill
    {
        public float percentShield;
        private bool _isUse;

        public override void Init()
        {
            _isUse = false;
            base.Init();
        }

        public override void Apply(WeaponStat stats)
        {
            base.Apply(stats);
            
            var player = Object.FindFirstObjectByType<PlayerManager>();
            if (player != null)
            {
                if (player.isUnder30)
                {
                    Handle();
                }
            }
            
            subscription = WorldMessenger.Sub<HealthUnder30>(Handle);
        }

        public override void Remove(WeaponStat stats)
        {
            base.Remove(stats);
            subscription?.Unsubscribe();
        }

        private void Handle()
        {
            if (_isUse)
                return;
            
            var shield = weaponStat.health.value * percentShield;
            WorldMessenger.Pub(new AddShieldMessage(shield));
            _isUse = true;
        }
    }
}