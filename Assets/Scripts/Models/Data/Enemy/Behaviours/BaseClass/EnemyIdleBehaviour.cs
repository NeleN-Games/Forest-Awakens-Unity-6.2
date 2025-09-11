using Interfaces.Enemy;
using UnityEngine;


namespace Models.Data.Enemy.Behaviours.BaseClass
{
    public abstract class EnemyIdleBehaviour : ScriptableObject, IEnemyBehavior.IIdleBehaviour
    {
        public virtual void Execute()
        {
        }
    }
}