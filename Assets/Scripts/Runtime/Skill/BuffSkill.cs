using Runtime.Stat;
using UnityEngine;

namespace Runtime.Skill
{
    [CreateAssetMenu(fileName = "Buff Skill", menuName = "Game SO/Skill Buff")]
    public class BuffSkill : BaseBuff
    {
        [SerializeReference] public BaseSkill baseSkill;

        public override void Apply(WeaponStat stats)
        {
            baseSkill.Apply(stats);
            base.Apply(stats);
        }

        public override void Remove(WeaponStat stats)
        {
            baseSkill.Remove(stats);
            base.Remove(stats);
        }
    }
}