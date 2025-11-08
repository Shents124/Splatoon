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
    public class ShredderBullet : BaseSkill
    {
        public float rate;
        public float attackPercent;
        
        public override void Apply(WeaponStat stats)
        {
            base.Apply(stats);
            subscription = WorldMessenger.Sub<BulletHitEnemy>(msg => Handle(msg.Position));
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

            var attack = attackPercent * weaponStat.attack.value;
            var direction =  Vector2.right;
            for (int i = 0; i < 2; i++)
            {
                direction *= -1;
                var clone = PoolService.Spawn<HorizontalBullet>(PoolType.Bullet, PrefabName.Horizontal_bullet);
                clone.transform.position = position;
                clone.Launch(attack, PrefabName.Horizontal_bullet, direction);
            }
        }
    }
}