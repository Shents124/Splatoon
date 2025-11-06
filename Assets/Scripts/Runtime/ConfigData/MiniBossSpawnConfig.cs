using System;
using UnityEngine;

namespace Runtime.ConfigData
{
    [CreateAssetMenu(fileName = "MiniBossSpawnConfig", menuName = "Game SO/MiniBossSpawnConfig")]
    public class MiniBossSpawnConfig : ScriptableObject
    {
        public SpawnNormalBallConfig[] spawnNormalBossConfigs;

        public SpawnNormalBallConfig GetConfig(int miniBossId)
        {
            foreach (var spawnNormalBossConfig in spawnNormalBossConfigs)
            {
                if (spawnNormalBossConfig.miniBossId == miniBossId)
                    return spawnNormalBossConfig;
            }

            return new SpawnNormalBallConfig();
        }
    }

    [Serializable]
    public struct SpawnNormalBallConfig
    {
        public int miniBossId;
        public float spawnInterval;
        public int ballId;
    }
}