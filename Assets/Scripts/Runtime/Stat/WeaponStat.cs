using Runtime.ConfigData;
using Runtime.Constant;
using Random = UnityEngine.Random;

namespace Runtime.Stat
{
    public class WeaponStat
    {
        public ModifiableStat attack;
        public ModifiableStat health;
        public ModifiableStat dmg;
        public ModifiableStat critRate;
        public ModifiableStat critDamage;
        public ModifiableStat fireRate;
        public ModifiableStat bulletPerShot;
        public ModifiableStat bulletSpeed;
        public ModifiableStat bulletSize;
        public ModifiableStat numberBounce;

        public void Initialize(WeaponConfig weaponConfig)
        {
            attack = new ModifiableStat(weaponConfig.attack);
            dmg = new ModifiableStat(weaponConfig.dmg);
            critRate = new ModifiableStat(weaponConfig.critRate);
            critDamage = new ModifiableStat(weaponConfig.critDamage);
            bulletPerShot = new ModifiableStat(weaponConfig.bulletPerShot);
            bulletSpeed = new ModifiableStat(weaponConfig.bulletSpeed);
            fireRate = new ModifiableStat(weaponConfig.fireRate);
            bulletSize = new ModifiableStat(1);
            numberBounce = new ModifiableStat(0);
        }
        
        public bool IsCrit()
        {
            var random = Random.Range(0, 1f);
            return random <= critRate.value;
        }

        public void AddModifier(StatType statType, StatModifier statModifier)
        {
            var stat = GetStat(statType);
            stat?.AddModifier(statModifier);
        }

        public void RemoveModifier(StatType statType, StatModifier statModifier)
        {
            var stat = GetStat(statType);
            stat?.RemoveModifier(statModifier);
        }

        private ModifiableStat GetStat(StatType statType)
        {
            switch (statType)
            {
                case StatType.Attack:
                    return attack;
                case StatType.Dmg:
                    return dmg;
                case StatType.CritRate:
                    return critRate;
                case StatType.CritDamage:
                    return critDamage;
                case StatType.Health:
                    return health;
                case StatType.BulletPerShot:
                    return bulletPerShot;
                case StatType.BulletSize:
                    return bulletSize;
                case StatType.FireRate:
                    return fireRate;
                case StatType.Bounce:
                    return numberBounce;
                default:
                    return null;
            }
        }
    }
}