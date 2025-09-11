using Interfaces.Enemy;
using UnityEngine;

namespace Models.Data.Enemy.Behaviours.BaseClass
{
    public abstract class EnemyAwareBehaviour : ScriptableObject, IEnemyBehavior.IAwarenessBehaviour
    {
        public virtual void Execute()
        {
        }
    }
}