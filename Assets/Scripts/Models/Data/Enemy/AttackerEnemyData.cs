using System;
using Enums;
using Interfaces.Enemy;
using Models.Structs;
using UnityEngine;

namespace Models.Data.Enemy
{
    [CreateAssetMenu(menuName = "Enemy/Attacker")]
    [Serializable]
    public class AttackerEnemyData : EnemyData
    {
        
        public override EnemyCategory GetEnum() => enumType;
    }
}