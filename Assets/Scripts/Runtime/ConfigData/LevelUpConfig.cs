using System;
using Runtime.Constant;
using UnityEngine;

namespace Runtime.ConfigData
{
    [CreateAssetMenu(fileName = "LevelUpConfig", menuName = "Game SO/LevelUpConfig")]
    public class LevelUpConfig : ScriptableObject
    {
        public LevelUpData[] levelUpData;

        public int maxLevel => levelUpData.Length;

        public int ExpRequired(int currentLevel)
        {
            return levelUpData[currentLevel].expRequired;
        }

        public BuffRarity BuffRarity(int currentLevel)
        {
            return levelUpData[currentLevel].raritySpawnType;
        }
    }

    [Serializable]
    public struct LevelUpData
    {
        public int expRequired;
        public BuffRarity raritySpawnType;
    }
}