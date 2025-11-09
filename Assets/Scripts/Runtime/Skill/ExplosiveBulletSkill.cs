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
    public class ExplosiveBulletSkill : BaseSkill
    {
        public float rate;
        public float attackPercent;
        public float radius;

        public override void Apply(WeaponStat stats)
        {
            base.Apply(stats);
            subscription = WorldMessenger.Sub<BulletHitEnemy>(msg => Handle(msg.position));
        }
        
        public override void Remove(WeaponStat stats)
        {
            subscription?.Unsubscribe();
        }

        private void Handle(Vector2 position)
        {
            var random = Random.Range(0, 1f);
            if (random > rate)
                return;

            var explosion = PoolService.Spawn<ExplosiveBullet>(PoolType.Bullet, PrefabName.Explosion_bullet);
            explosion.transform.position = position;
            var damage = attackPercent * weaponStat.attack.value;
            explosion.Initialize(PrefabName.Explosion_bullet, damage, radius);
        }
    }
}