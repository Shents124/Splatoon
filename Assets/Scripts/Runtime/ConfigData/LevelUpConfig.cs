using UnityEngine;

namespace Runtime.ConfigData
{
    [CreateAssetMenu(fileName = "LevelUpConfig", menuName = "Game SO/LevelUpConfig")]
    public class LevelUpConfig : ScriptableObject
    {
        public int[] expRequired;

        public int maxLevel => expRequired.Length;
    }
}