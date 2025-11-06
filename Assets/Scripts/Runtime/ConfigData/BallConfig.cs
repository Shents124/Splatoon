using System;
using System.Collections.Generic;
using Runtime.Constant;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Runtime.ConfigData
{
    [CreateAssetMenu(fileName = "Ball Config", menuName = "Game SO/Ball Config")]
    public class BallConfig : SerializedScriptableObject
    {
        [OdinSerialize]
        public Dictionary<int, BallConfigData> ballConfig = new();

        public BallConfigData GetBallConfig(int ballId)
        {
            return ballConfig[ballId];
        }
    }

    [Serializable]
    public struct BallConfigData
    {
        public int id;
        public Vector2 randomSize;
        public int exp;
        public int coin;

        public float GetRandomScale(BallType ballType)
        {
            if (ballType == BallType.Normal)
                return UnityEngine.Random.Range(randomSize.x, randomSize.y);
            
            return randomSize.y;
        }
    }
}