using System;
using Enums;
using UnityEngine;

namespace Models.Structs
{
    [Serializable]
    public struct EnemyType
    { 
        public EnemyCategory Category { get; private set; }
        public EnemySubCategory Type { get; private set; }

        public EnemyType(EnemyCategory category, EnemySubCategory type)
        {
            if (!IsValid(category, type))
                throw new Exception($"Invalid combination: {category} + {type}");

            Category = category;
            Type = type;
        }

        private static bool IsValid(EnemyCategory category, EnemySubCategory type)
        {
            return category switch
            {
                EnemyCategory.Attacker => type == EnemySubCategory.Melee
                                          || type == EnemySubCategory.Ranged
                                          || type == EnemySubCategory.Area
                                          || type == EnemySubCategory.Dash,

                EnemyCategory.Defender => type == EnemySubCategory.ShieldBearer
                                          || type == EnemySubCategory.Tank,

                EnemyCategory.Supporter => type == EnemySubCategory.Healer
                                           || type == EnemySubCategory.Buffer,

                EnemyCategory.Escaper => type == EnemySubCategory.Runner
                                         || type == EnemySubCategory.Teleporter,

                _ => false
            };
        }

    }
}
