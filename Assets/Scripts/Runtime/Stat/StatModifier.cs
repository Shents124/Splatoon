using Runtime.Constant;

namespace Runtime
{
    public class StatModifier
    {
        public float value;
        public ModifierType type;
        
        public StatModifier(float value, ModifierType type)
        {
            this.value = value;
            this.type = type;
        }
    }
}