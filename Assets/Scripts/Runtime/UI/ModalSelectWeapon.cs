using System;
using System.Collections.Generic;
using Runtime.ConfigData;
using UnityEngine;

namespace Runtime.UI
{
    public class ModalSelectWeapon : MonoBehaviour
    {
        [SerializeField]
        private List<SelectedWeaponUI> _selectedWeaponUis;

        public ModalSelectWeapon(List<SelectedWeaponUI> selectedWeaponUis)
        {
            _selectedWeaponUis = selectedWeaponUis;
        }

        public void SetSelected(Action<WeaponConfig> selected)
        {
            foreach (var selectedWeaponUi in _selectedWeaponUis)
            {
                selectedWeaponUi.SetOnSelected(selected);
            }
        }
    }
}