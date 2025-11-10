using System;
using Runtime.ConfigData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI
{
    public class SelectedWeaponUI : MonoBehaviour
    {
        public WeaponConfig weaponConfig;
        public TextMeshProUGUI weaponName;
        public TextMeshProUGUI attack;
        public TextMeshProUGUI dmg;
        public TextMeshProUGUI critRate;
        public TextMeshProUGUI critDamage;
        public TextMeshProUGUI fireRate;
        public TextMeshProUGUI bulletPerShot;
        public TextMeshProUGUI bulletRange;
        public Image image;

        public Button onClickedButton;
        private Action<WeaponConfig> _onSelected;

        private void Start()
        {
            weaponName.text = weaponConfig.weaponName;
            attack.text = $"Attack: {weaponConfig.attack}";
            dmg.text = $"Dmg: {weaponConfig.dmg * 100}%";
            critRate.text = $"Crit Rate: {weaponConfig.critRate * 100}%";
            critDamage.text = $"Crit Damage: {weaponConfig.critDamage * 100}%";
            fireRate.text = $"Fire Rate: {weaponConfig.fireRate}";
            bulletPerShot.text = $"Bullet per shot: {weaponConfig.bulletPerShot}";
            bulletRange.text = $"Bullet Range: {weaponConfig.bulletRange}";
            image.sprite = weaponConfig.sprite;
            onClickedButton.onClick.AddListener(OnClick);
        }

        public void SetOnSelected(Action<WeaponConfig> onSelected)
        {
            _onSelected = onSelected;
        }

        private void OnClick()
        {
            _onSelected?.Invoke(weaponConfig);
        }
    }
}