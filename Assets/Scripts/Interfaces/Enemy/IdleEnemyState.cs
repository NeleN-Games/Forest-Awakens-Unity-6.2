using Enums;
using Models.Data;
using Models.Data.Enemy;
using UnityEngine;

namespace Interfaces.Enemy
{
    public class IdleEnemyState : IEnemyState
    {
        private readonly EnemyController controller;
        private readonly EnemyData data;

        public IdleEnemyState(EnemyController controller, EnemyData data)
        {
            this.controller = controller;
            this.data = data;
        }

        public void Enter() { Debug.Log("Entered Idle State"); }

        public void Update()
        {
            data.IdleBehavior?.Execute(); // از دیتا اجرا میشه
            // شرط تغییر State (مثلا اگر بازیکن نزدیک شد):
            /*if (Vector3.Distance(controller.transform.position, Player.Instance.transform.position) < 5f)
            {
                controller.StateMachine.ChangeState(new AwarenessState(controller, data));
            }*/
        }

        public void Exit() { Debug.Log("Exit Idle State"); }
    }
}