using System;
using Models.Data.Enemy.Behaviours.BaseClass;
using UnityEngine;

namespace Models.Data.Enemy.Behaviours
{
    [CreateAssetMenu(menuName = "Enemy/Behaviors/Awareness/Simple Awareness")]
    [Serializable]

    public class SimpleAwarenessBehaviour: EnemyAwareBehaviour
    {
        public override void Execute()
        {
            
        }
    }
}