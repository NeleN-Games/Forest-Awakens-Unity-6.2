using System;
using Interfaces.Enemy;
using UnityEngine;

namespace Models.Data.Enemy
{
    [CreateAssetMenu(menuName = "Enemy/Behaviors/Awareness/Simple Awareness")]
    [Serializable]

    public class SimpleAwarenessBehaviour: ScriptableObject, IEnemyBehavior.IAwarenessBehaviour 
    {
        public void Execute()
        {
        }
    }
}