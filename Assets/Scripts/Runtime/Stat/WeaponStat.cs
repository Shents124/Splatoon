using Runtime.ConfigData;
using UnityEngine;

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

        public void Initialize(WeaponConfig weaponConfig)
        {
            attack = new ModifiableStat(weaponConfig.attack);
            dmg = new ModifiableStat(weaponConfig.dmg);
            critRate = new ModifiableStat(weaponConfig.critRate);
            critDamage = new ModifiableStat(weaponConfig.critDamage);
            bulletPerShot = new ModifiableStat(weaponConfig.bulletPerShot);
            bulletSpeed = new ModifiableStat(weaponConfig.bulletSpeed);
        }

        public bool IsCrit()
        {
            var random = Random.Range(0, 1f);
            return random <= critRate.value;
        }
    }
}