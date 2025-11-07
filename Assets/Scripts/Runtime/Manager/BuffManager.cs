using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Runtime.Constant;
using Runtime.Service;
using Runtime.Skill;
using Runtime.Stat;
using UnityEngine;
using ZLinq;

namespace Runtime.Manager
{
    public class BuffManager : MonoBehaviour
    {
        [SerializeField] private List<BaseBuff> buffLevel1 = new();
        [SerializeField] private List<BaseBuff> buffLevel2 = new();
        [SerializeField] private List<BaseBuff> otherBuffs = new();
        
        private List<BaseBuff> _buffs = new();

        private WeaponStat _weaponStat;

        public void SetWeaponStat(WeaponStat weaponStat)
        {
            _weaponStat = weaponStat;
        }
        
        public void ShowSelectBuff(BuffRarity buffRarity)
        {
            Time.timeScale = 0;

            var poolLevel2Ids = new List<int>();
            foreach (var buff in _buffs)
            {
                if (buff.upgradeId > 0)
                {
                    poolLevel2Ids.Add(buff.upgradeId);
                }
            }
            
            var poolBuffLevel2 = buffLevel2.Where(x => poolLevel2Ids.Contains(x.upgradeId)).ToList();

            var poolBuff = new List<BaseBuff>();
            poolBuff.AddRange(buffLevel1);
            poolBuff.AddRange(poolBuffLevel2);
            poolBuff.AddRange(otherBuffs);
            
            var buffShows = poolBuff.Where(x => x.rarity == buffRarity).Except(_buffs).Shuffle().Take(3).ToList();

            var data = new ShowBuffModalData()
            {
                buffs = buffShows,
                onSelected = OnSelectedBuff
            };
            
            UiService.OpenModalAsync(ModalType.ModalShowBuff, args: data).Forget();
        }

        private void OnSelectedBuff(BaseBuff selectedBuff)
        {
            var indexRemove = -1;
            for (var i = 0; i < _buffs.Count; i++)
            {
                var existingBuff = _buffs[i];
                if (existingBuff.Equals(selectedBuff))
                    break;

                if (existingBuff.upgradeId == selectedBuff.id)
                {
                    indexRemove = i;
                    break;
                }
            }
            
            if (indexRemove != -1)
            {
                var removeBuff = _buffs[indexRemove];
                _buffs.RemoveAt(indexRemove);
                RemoveBuff(removeBuff);
                removeBuff.Remove(_weaponStat);
            }
            
            _buffs.Add(selectedBuff);
            selectedBuff.Apply(_weaponStat);
            
            Time.timeScale = 1;
        }

        private void RemoveBuff(BaseBuff buff)
        {
            if (buffLevel1.Contains(buff))
                buffLevel1.Remove(buff);
            if (buffLevel2.Contains(buff))
                buffLevel2.Remove(buff);
            if (otherBuffs.Contains(buff))
                otherBuffs.Remove(buff);
        }
    }
}