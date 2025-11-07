using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI
{
    public class LevelUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI levelTxt;
        [SerializeField] private Slider slider;
        
        public void UpdateData(int level, float value, bool isMax = false)
        {
            if (isMax)
            {
                levelTxt.text = "MAX";
                slider.value = 1;
            }
            else
            {
                levelTxt.text = $"Level {level}";
                slider.value = value;
            }
        }
    }
}