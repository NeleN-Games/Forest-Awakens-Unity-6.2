using System;
using Interfaces.Enemy;
using Models.Data.Enemy.Behaviours.BaseClass;
using UnityEngine;

namespace Models.Data.Enemy.Behaviours
{
    [CreateAssetMenu(menuName = "Enemy/Behaviors/Attacker/Range")]
    [Serializable]

    public class RangeAttackerBehavior:EnemyActBehaviour
    {
        public override void Execute()
        {
           
        }

        public float Damage { get; set; }
        public float AttackInterval { get; set; }
        public Transform Target { get; set; }
    }
}