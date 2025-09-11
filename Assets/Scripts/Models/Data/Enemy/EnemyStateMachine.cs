using Enums;
using Interfaces.Enemy;

namespace Models.Data.Enemy
{
    public class EnemyStateMachine
    {
        private IEnemyState currentState;
        private readonly EnemyController controller;
        private readonly EnemyData data;

        public EnemyStateMachine(EnemyController controller, EnemyData data)
        {
            this.controller = controller;
            this.data = data;
        }

        public void InitializeIdle()
        {
            ChangeState(new IdleEnemyState(controller, data));
        }

        public void ChangeState(IEnemyState newState)
        {
            currentState?.Exit();
            currentState = newState;
            currentState.Enter();
        }

        public void Update()
        {
            currentState?.Update();
        }
    }
}