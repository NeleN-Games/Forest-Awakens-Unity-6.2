using UnityEngine;

namespace Interfaces.Enemy
{
    
    public interface IAttackerEnemyBehaviour : IEnemyBehavior.IActBehaviour
    {
        public float Damage { get; set; }
        public float AttackInterval { get; set; }
        public Transform Target { get; set; }
    }
}