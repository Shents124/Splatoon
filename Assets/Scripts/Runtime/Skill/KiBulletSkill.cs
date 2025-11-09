using System;
using Runtime.Constant;
using Runtime.Pool;
using Runtime.PubSub;
using Runtime.PubSub.CommonMessage;
using Runtime.Stat;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.Skill
{
    [Serializable]
    public class KiBulletSkill : BaseSkill
    {
        public float damagePercent;
        public float rate;
        
        public override void Apply(WeaponStat stats)
        {
            base.Apply(stats);
            subscription = WorldMessenger.Sub<BulletHitEnemy>(msg => Handle(msg.position));
        }

        private void Handle(Vector2 position)
        {
            var random = Random.Range(0, 1f);
            if (random > rate)
                return;
            
            var clone = PoolService.Spawn<KiBullet>(PoolType.Bullet, PrefabName.ki_bullet);
            clone.transform.position = position;
            clone.Launch(PrefabName.ki_bullet, damagePercent * weaponStat.attack.value);
        }
    }
}