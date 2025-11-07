using System.Collections.Generic;
using Extensions;
using Runtime.Skill;
using UnityEngine;

namespace Runtime.Manager
{
    public class BuffManager : MonoBehaviour
    {
        [SerializeField] private List<BaseBuff> poolBuff = new();
        
        private List<BaseBuff> _buffs = new List<BaseBuff>();

        public void ShowSelectBuff()
        {
            
        }
        
        private List<BaseBuff> GetGachaBuff()
        {
            var pool = new List<BaseBuff>();
            foreach (var baseBuff in poolBuff)
            {
                if (_buffs.Contains(baseBuff))
                    continue;
                
                pool.Add(baseBuff);
            }
            
            if (pool.Count <= 3)
                return pool;

            return pool.GetRandomElements(3);
        }
    }
}