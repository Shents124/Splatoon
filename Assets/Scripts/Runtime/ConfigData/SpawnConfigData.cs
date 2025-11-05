using System;
using System.Collections.Generic;
using Runtime.Constant;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Runtime.ConfigData
{
    [CreateAssetMenu(fileName = "SpawnConfigData", menuName = "Game SO/SpawnConfigData")]
    public class SpawnConfigData : SerializedScriptableObject
    {
        [OdinSerialize]
        public List<List<BallSpawnConfig>> waveConfigs = new();

        public List<BallSpawnConfig> GetWaveConfig(int waveId)
        {
            return waveConfigs[waveId];
        }
    }
    
    [Serializable]
    public struct BallSpawnConfig
    {
        public int ballId;
        public BallType ballType;
        public float heath;
        public float attack;
        public int count;
    }
}