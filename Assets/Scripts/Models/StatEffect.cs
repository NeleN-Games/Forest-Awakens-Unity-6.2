using System;
using Enums;
using UnityEngine.Serialization;

namespace Models
{
    [Serializable]
    public class StatEffect
    {
        public StatType statType;
        public float amount;
        public bool hasEffectRate;
        public float rateAmount;
        public float duration = 0f;
    }
}