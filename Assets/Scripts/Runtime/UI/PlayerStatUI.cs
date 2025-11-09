using TMPro;
using UnityEngine;

namespace Runtime.UI
{
    public class PlayerStatUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI hpTxt;
        [SerializeField] private TextMeshProUGUI shieldTxt;

        public void UpdateHealth(float value)
        {
            hpTxt.text = $"Hp: {(int)value}";
        }

        public void UpdateShield(float value)
        {
            shieldTxt.text = $"Shield: {(int)value}";
        }
    }
}