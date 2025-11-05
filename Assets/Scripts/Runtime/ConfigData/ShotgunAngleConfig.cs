using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Runtime.ConfigData
{
    [CreateAssetMenu(fileName = "ShotgunAngleConfig", menuName = "Game SO/ShotgunAngleConfig")]
    public class ShotgunAngleConfig : SerializedScriptableObject
    {
        [OdinSerialize]
        private Dictionary<int, int> _angleShotgun = new Dictionary<int, int>();

        public int GetAngle(int bulletCount)
        {
            return _angleShotgun[bulletCount];
        }
    }
}