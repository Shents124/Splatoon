using Runtime.Interface;
using UnityEngine;

namespace Runtime.Skill
{
    public class SpikeShield : MonoBehaviour, IDamageable
    {
        private float _damage;

        public void Initialize(float damage)
        {
            _damage = damage;
        }
        public float GetDamage()
        {
            return _damage;
        }
    }
}