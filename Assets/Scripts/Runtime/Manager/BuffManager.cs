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
        [SerializeField] private List<BaseBuff> poolBuffUpgrade= new();
        [SerializeField] private List<BaseBuff> pollBuffNotUpgrade = new();
        
        [SerializeField] private List<BaseBuff> poolValidBuff = new();
        private List<BaseBuff> _buffs = new();

        private WeaponStat _weaponStat;

        public void SetWeaponStat(WeaponStat weaponStat)
        {
            _weaponStat = weaponStat;
        }
        
        public void ShowSelectBuff(BuffRarity buffRarity)
        {
            Time.timeScale = 0;

            poolValidBuff = new List<BaseBuff>();

            foreach (var baseBuff in poolBuffUpgrade)
            {
                if (!HasBuffFromFamily(baseBuff.rootId) && baseBuff.upgradeId > 0)
                {
                    poolValidBuff.Add(baseBuff);
                }
                
                if (HasBuffFromFamily(baseBuff.rootId))
                {
                    if (baseBuff.upgradeId < 0)
                    {
                        poolValidBuff.Add(baseBuff);
                    }
                }
            }
            
            poolValidBuff.AddRange(pollBuffNotUpgrade);
            
            var buffShows = poolValidBuff.Where(x => x.rarity == buffRarity).Except(_buffs).Shuffle().Take(3).ToList();

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

                if (existingBuff.rootId == selectedBuff.rootId)
                {
                    if (existingBuff.upgradeId == selectedBuff.id)
                    {
                        indexRemove = i;
                        break;
                    }
                }
            }
            
            if (indexRemove != -1)
            {
                var removeBuff = _buffs[indexRemove];
                _buffs.RemoveAt(indexRemove);
                removeBuff.Remove(_weaponStat);
            }
            
            _buffs.Add(selectedBuff);
            selectedBuff.Apply(_weaponStat);
            
            Time.timeScale = 1;
        }
        
        private bool HasBuffFromFamily(int rootId)
        {
            return _buffs.Exists(b => b.rootId == rootId);
        }

        private bool HaveBuff(int buffId)
        {
            return _buffs.Exists(b => b.id == buffId);
        }
    }
}