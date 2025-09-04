using System;
using Base_Classes;
using Models;
using Systems;
using UnityEngine;

namespace Models
{
    [RequireComponent(typeof(HungerSystem))]
    public class HungerStat : StatBase
    {
        protected override void ModifyStat(StatEffect effect)
        {
            base.ModifyStat(effect);
        }
    }
}