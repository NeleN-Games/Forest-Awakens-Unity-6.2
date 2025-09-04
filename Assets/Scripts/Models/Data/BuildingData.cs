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
        public Vector2 buildingSize;
        public override BuildingType GetEnum() => enumType;
        public override void Craft()
        {
            Debug.Log("Crafting Building");
        }

        public override void Initialize(GameObject prefab, Sprite icon, BuildingType enumType, List<SourceRequirement> resourceRequirements,
            CategoryType categoryType, UniqueId uniqueId, CraftableAvailabilityState craftableAvailabilityState)
        {
            base.Initialize(prefab, icon, enumType, resourceRequirements, categoryType, uniqueId, craftableAvailabilityState);
            
        }
    }
}
