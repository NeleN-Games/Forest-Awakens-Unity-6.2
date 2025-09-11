namespace Interfaces.Enemy
{
    
    public interface IEnemyBehavior
    {
        void Execute();
        public interface IIdleBehaviour : IEnemyBehavior
        {
        }

        public interface IAwarenessBehaviour : IEnemyBehavior
        {
        }

        public interface IActBehaviour : IEnemyBehavior
        {
        }
    }
}