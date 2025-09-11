using System;
using DG.Tweening;
using Interfaces.Enemy;
using Models.Data.Enemy.Behaviours.BaseClass;
using UnityEngine;

namespace Models.Data.Enemy.Behaviours
{
    [CreateAssetMenu(menuName = "Enemy/Behaviors/Idle/Line Walking")]
    [Serializable]
    public class LineWalkingBehaviour: EnemyIdleBehaviour
    { 
        public Transform pointA;
        public Transform pointB;
        public Transform currentTransform;
        public bool isMoveToA;
        public bool isMoving;
        public float speed;
        public override void Execute()
        {
            if (isMoving) return;
            float distance = 0;
            if(isMoveToA)
            {
                distance = Vector3.Distance(currentTransform.position, pointA.position);
                if (Mathf.Approximately(distance,0))
                {
                    isMoveToA = false;
                }
                else
                {
                    currentTransform.DOMove(pointA.position, 1/speed).OnComplete(()=> isMoving=false);
                    isMoving=true;
                }
            }
            else
            {
                distance = Vector3.Distance(currentTransform.position, pointB.position);
                if (Mathf.Approximately(distance,0))
                {
                    isMoveToA = true;
                }
                else
                {
                    currentTransform.DOMove(pointB.position, 1/speed).OnComplete(()=> isMoving=false);
                    isMoving=true;

                }
            }
            

        }
    }
}