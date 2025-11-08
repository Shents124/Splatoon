using UnityEngine;

namespace Runtime.PubSub.CommonMessage
{
    public struct BulletHitEnemy
    {
        public readonly Vector2 Position;

        public BulletHitEnemy(Vector2 position)
        {
            Position = position;
        }
    }
}