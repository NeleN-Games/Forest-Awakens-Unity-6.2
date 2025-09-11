using System;
using Interfaces.Enemy;
using UnityEngine;

namespace Models.Data.Enemy
{
    [CreateAssetMenu(menuName = "Enemy/Behaviors/Attacker/Range")]
    [Serializable]

    public class RangeAttackerBehavior: ScriptableObject, IAttackerEnemyBehaviour
    {
        public void Execute()
        {
           
        }

        public float Damage { get; set; }
        public float AttackInterval { get; set; }
        public Transform Target { get; set; }
    }
}