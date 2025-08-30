using Base_Classes;
using Models;
using Systems;
using UnityEngine;

namespace Models
{
    [RequireComponent(typeof(HealthSystem))]
    public class HealthStat : StatBase
    {
        protected override void ModifyStat(StatEffect effect)
        {
            base.ModifyStat(effect);
        }
    }
}
