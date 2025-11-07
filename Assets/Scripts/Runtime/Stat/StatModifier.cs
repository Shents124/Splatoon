using Runtime.Constant;

namespace Runtime.Stat
{
    public class StatModifier
    {
        public StatType statType;
        public float value;
        public ModifierType type;
        
        public StatModifier(StatType statType, float value, ModifierType type)
        {
            this.statType = statType;
            this.value = value;
            this.type = type;
        }
    }
}