using System;
using System.Collections.Generic;
using Models;

namespace Models
{
    [Serializable]
    public class StatModifier
    {
        public List<StatEffect> effects = new List<StatEffect>();
    }
}