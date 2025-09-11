using Interfaces.Enemy;
using UnityEngine;

namespace Models.Data.Enemy.Behaviours.BaseClass
{
    public abstract class EnemyActBehaviour: ScriptableObject, IEnemyBehavior.IActBehaviour
    {
        public virtual void Execute()
        {
        }
    }
}