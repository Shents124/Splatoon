using System.Collections.Generic;
using Runtime.Skill;
using UnityEngine;

namespace Runtime.Manager
{
    public class BuffManager : MonoBehaviour
    {
        public List<BaseBuff> buffs = new List<BaseBuff>();
        private WeaponStat _weaponStat;
        
        public void AddBuff(BaseBuff newBuff)
        {
            for (int i = 0; i < buffs.Count; i++)
            {
                var exitBuff = buffs[i];
                if (exitBuff.id == newBuff.id)
                    return;

                if (exitBuff.upgradeId == newBuff.id)
                {
                    buffs.RemoveAt(i);
                    exitBuff.Remove(_weaponStat);
                    break;
                }
            }
            
            buffs.Add(newBuff);
            newBuff.Apply(_weaponStat);
        }
    }
}