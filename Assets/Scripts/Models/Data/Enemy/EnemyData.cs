using System;
using Enums;
using Interfaces;
using Interfaces.Enemy;
using Models.Structs;
using UnityEngine;

namespace Models.Data
{
    [Serializable]
    public abstract class EnemyData : CommonAssetData<EnemyCategory>
    
    {
        public float health;
        public EnemyType enemyType;
        
        [SerializeField] public IEnemyBehavior.IIdleBehaviour IdleBehavior;
        [SerializeField] public IEnemyBehavior.IAwarenessBehaviour AwareBehavior;
        [SerializeField] public IEnemyBehavior.IActBehaviour ActBehavior;
        

        public void Idle()
        {
            IdleBehavior?.Execute();
        }  
        public void Aware()
        {
            AwareBehavior?.Execute();
        }
        public void Act()
        {
            ActBehavior?.Execute();
        }
        public virtual void Initialize(GameObject prefab, Sprite icon, float health,EnemyType enemyType
        ,IEnemyBehavior.IIdleBehaviour idleBehavior,IEnemyBehavior.IAwarenessBehaviour awareBehavior
        ,IEnemyBehavior.IActBehaviour actBehavior
        )
        {
            enumType =enemyType.Category;
            base.Initialize(prefab, icon, enumType);
            this.health = health;
            this.enemyType = enemyType;
            this.IdleBehavior = idleBehavior;
            this.AwareBehavior = awareBehavior;
            this.ActBehavior = actBehavior;
        }

        public override CommonAssetData<EnemyCategory> Clone()
        {
            return Instantiate(this);
        }

        public void CheckHealth()
        {
            if (health <= 0)
                Die();
        }
        protected virtual void Die()
        {
            
        }

    }
}
