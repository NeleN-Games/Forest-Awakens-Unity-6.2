using System;
using System.Collections.Generic;
using Enums;
using Interfaces;
using UnityEngine;

namespace Models.Data
{
    [CreateAssetMenu(menuName = "Data/Building")]
    [Serializable]
    public class BuildingData : CraftableAssetData<BuildingType>
    {
        public override BuildingType GetEnum() => enumType;
        public override void Craft()
        {
            Debug.Log("Crafting Building");
        }
    }
}
