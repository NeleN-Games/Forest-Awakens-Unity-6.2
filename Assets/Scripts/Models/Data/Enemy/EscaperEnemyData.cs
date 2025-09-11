using System;
using Enums;
using Interfaces.Enemy;

namespace Models.Data.Enemy
{
    public class EscaperEnemyData: EnemyData
    {
        
        public override EnemyCategory GetEnum() => enumType;
    }
}