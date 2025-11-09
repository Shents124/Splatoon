using UnityEngine;

namespace Runtime.PubSub.CommonMessage
{
    public struct BulletHitEnemy
    {
        public readonly Vector2 position;

        public BulletHitEnemy(Vector2 position)
        {
            this.position = position;
        }
    }
    
    public struct HealthUnder30 {}

    public struct AddShieldMessage
    {
        public readonly float value;
        public AddShieldMessage(float value)
        {
            this.value = value;
        }
    }

    public struct SpawnSpikeShieldMessage
    {
        public float shield;
        public float damage;
    }

    public struct SpawnDroneMessage
    {
        public int numberDrone;
        public float damage;
        public float fireRate;
    }
}