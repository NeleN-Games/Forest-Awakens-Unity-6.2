using Enums;
using Interfaces.Enemy;
using UnityEngine;

namespace Models.Data.Enemy
{
    public abstract class EnemyController: MonoBehaviour
    {
        public EnemyStateMachine StateMachine { get; private set; }

        public EnemyData Data;

        protected virtual void Awake()
        {
            StateMachine = new EnemyStateMachine(this, Data);
            StateMachine.InitializeIdle();
        }

        protected virtual void Update()
        {
            StateMachine.Update();
        }
    }
}