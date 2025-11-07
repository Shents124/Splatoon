using System;
using Runtime.Constant;
using UnityEngine;

namespace Runtime.Service
{
    public static class Helper
    {
        public static Color GetColorByBuffRarity(BuffRarity buffRarity)
        {
            switch (buffRarity)
            {
                case BuffRarity.Normal:
                    return Color.white;
                case BuffRarity.Legendary:
                    return Color.yellow;
                case BuffRarity.Mythic:
                    return Color.red;
            }
            
            return Color.white;
        }
    }
}