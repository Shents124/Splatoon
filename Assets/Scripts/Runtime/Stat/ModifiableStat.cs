using System.Collections.Generic;
using Runtime.Constant;
using UnityEngine;

namespace Runtime.Stat
{
    public class ModifiableStat
    {
        private readonly float _baseValue;
        
        private readonly List<StatModifier> _modifiers;
        
        private float _currentValue;

        public float value => _currentValue;

        public ModifiableStat(float baseValue)
        {
            _baseValue = baseValue;
            _modifiers = new();
            Calculate();
        }
        
        private void Calculate()
        {
            var finalValue = _baseValue;
            float sumAdd = 0;
            float sumMul = 0;
            
            foreach (var mod in _modifiers)
            {
                switch (mod.type)
                {
                    case ModifierType.Additive:
                        sumAdd += mod.value;
                        break;

                    case ModifierType.Multiplicative:
                        sumMul += mod.value;
                        break;
                }
            }

            finalValue += sumAdd;
            finalValue *= (1 + sumMul);
            
            _currentValue = finalValue;
        }
        
        public void AddModifier(StatModifier mod)
        {
            _modifiers.Add(mod);
            Calculate();
            Debug.Log($"{mod.statType}: {_currentValue}");
        }

        public void RemoveModifier(StatModifier mod)
        {
            _modifiers.Remove(mod);
            Calculate();
            Debug.Log($"{mod.statType}: {_currentValue}");
        }
        
        public void ClearModifiers()
        {
            _modifiers.Clear();
        }
    }
}