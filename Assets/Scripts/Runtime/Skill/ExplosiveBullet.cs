using System.Collections;
using Runtime.Interface;
using Runtime.Pool;
using UnityEngine;

namespace Runtime.Skill
{
    public class ExplosiveBullet : MonoBehaviour, IDamageable
    {
        public float lifeTime = 0.1f;
        

        private string _key;
        private float _damage;
        private Coroutine _coroutine;
        
        public void Initialize(string key, float damage, float size)
        {
            _key = key;
            _damage = damage;
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
            
            StartCoroutine(nameof(AutoHie));
            gameObject.transform.localScale = new Vector3(size, size, size);
        }

        private IEnumerator AutoHie()
        {
            yield return new WaitForSeconds(lifeTime);
            PoolService.Despawn(PoolType.Bullet, _key, gameObject);
            _coroutine = null;
        }

        public float GetDamage()
        {
            return _damage;
        }
    }
}