using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Runtime.Service;
using Runtime.Skill;
using Runtime.UI;
using UnityEngine;
using ZBase.UnityScreenNavigator.Core.Modals;

namespace Runtime.Manager
{
    public class ShowBuffModalData
    {
        public List<BaseBuff> buffs;
        public Action<BaseBuff> onSelected;
    }
    
    public class ShowBuffManager : Modal
    {
        [SerializeField] private GameObject blockClick;
        [SerializeField] private List<BuffUI> buffUis = new List<BuffUI>();

        private ShowBuffModalData _data;
        private BaseBuff _buffSelected;
        
        public override UniTask Initialize(Memory<object> args)
        {
            if (args.IsEmpty)
            {
                foreach (var buffUi in buffUis)
                {
                    buffUi.gameObject.SetActive(false);
                }
            }
            else
            {
                blockClick.gameObject.SetActive(true);
                _data = (ShowBuffModalData)args.Span[0];
                var buffs = _data.buffs;
                for (var i = 0; i < buffUis.Count; i++)
                {
                    if (i < buffs.Count)
                    {
                        buffUis[i].gameObject.SetActive(true);
                        buffUis[i].ShowBuff(buffs[i], OnSelected);
                    }
                    else
                    {
                        buffUis[i].gameObject.SetActive(false);
                    }
                }
            }
            return base.Initialize(args);
        }
        
        public override void DidPushEnter(Memory<object> args)
        {
            DisableBlockClick().Forget();
            base.DidPushEnter(args);
        }

        public override void DidPopExit(Memory<object> args)
        {
            _data?.onSelected?.Invoke(_buffSelected);
            base.DidPopExit(args);
        }

        private async UniTask DisableBlockClick()
        {
            await UniTask.Delay(100, DelayType.UnscaledDeltaTime);
            blockClick.gameObject.SetActive(false);
        }
        
        private void OnSelected(BaseBuff baseBuff)
        {
            _buffSelected = baseBuff;
            UiService.CloseModal();
        }
    }
}