using UnityEngine;

namespace Runtime.Pool
{
    public interface IDeSpawn
    {
        void OnDeSpawn();
        GameObject ObjectDeSpawn();
    }
}