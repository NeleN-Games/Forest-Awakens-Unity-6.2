using Base_Classes;
using Models;
using Systems;
using UnityEngine;

namespace Models
{
    [RequireComponent(typeof(EnergySystem))]
    public class EnergyStat : StatBase
    {
        protected override void ModifyStat(StatEffect effect)
        {
            base.ModifyStat(effect);
        }
    }
}
