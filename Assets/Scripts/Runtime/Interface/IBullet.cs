using UnityEngine;

namespace Runtime.Interface
{
    public interface IBullet : IDamageable
    {
        void Despawn();
        Vector2 position { get; }
    }
}