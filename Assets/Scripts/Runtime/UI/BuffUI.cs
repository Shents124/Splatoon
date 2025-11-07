using System;
using Runtime.Service;
using Runtime.Skill;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI
{
    public class BuffUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI buffName;
        [SerializeField] private TextMeshProUGUI buffDescription;
        [SerializeField] private Image holder;
        [SerializeField] private Button onSelectedBtn;
        
        private BaseBuff _buff;
        private Action<BaseBuff> _onSelected;
        
        private void Awake()
        {
            onSelectedBtn.onClick.AddListener(OnSelected);
        }

        public void ShowBuff(BaseBuff baseBuff, Action<BaseBuff> onSelected)
        {
            _buff = baseBuff;
            _onSelected = onSelected;
            buffName.text = baseBuff.id.ToString();
            buffDescription.text = baseBuff.description;
            holder.color = Helper.GetColorByBuffRarity(baseBuff.rarity);
        }

        private void OnSelected()
        {
            _onSelected?.Invoke(_buff);
        }
    }
}