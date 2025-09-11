using System;
using Enums;
using Interfaces.Enemy;

namespace Models.Data.Enemy
{
    public class DefenderEnemyData : EnemyData
    {
        public override EnemyCategory GetEnum() => enumType;
    }
}