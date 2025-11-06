using Runtime.Constant;
using UnityEngine;

namespace Runtime.Skill
{
    [CreateAssetMenu(fileName = "New Skill Stat Buff", menuName = "GameSO/Skill Stat Buff")]
    public class StatBuff : BaseBuff, IBuff
    {
        public StatBuffConfig[] buff;
    }

    public struct StatBuffConfig
    {
        public BuffType buffType;
        public float value;
    }
}