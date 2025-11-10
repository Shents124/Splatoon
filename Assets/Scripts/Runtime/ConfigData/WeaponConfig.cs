using Runtime.Constant;
using UnityEngine;

namespace Runtime.ConfigData
{
    [CreateAssetMenu(fileName = "Weapon Config", menuName = "Game SO/WeaponConfig")]
    public class WeaponConfig : ScriptableObject
    {
        public int id;
        public string weaponName;
        public WeaponType weaponType;
        public int attack;
        public float dmg;
        public float critRate;
        public float critDamage;
        public float fireRate;
        public int bulletPerShot;
        public float bulletSpeed;
        public float bulletLifeTime;
        public float bulletRange;
        public float knockback;
    }
}