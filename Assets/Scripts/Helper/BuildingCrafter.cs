using System;
using Databases;
using Enums;
using Managers;
using Models.Data;
using Services;
using UnityEngine;

namespace Helper
{
    public class BuildingCrafter : Crafter<BuildingType, BuildingData, BuildingDatabase>
    {

        [SerializeField] private BuildingPlacer buildingPlacer;
        public Action<BuildingData,Vector3> OnPlacingBuilding;
        public void Awake()
        {
            Database = ServiceLocator.Get<BuildingDatabase>();
            OnCraft -= Craft;
            OnCraft += Craft;
            OnPlacingBuilding -= PlaceBuilding;
            OnPlacingBuilding += PlaceBuilding;
        }

        public void OnDestroy()
        {
            OnCraft -= Craft;
        }

        protected override void HandleCraft(BuildingData data)
        {
            buildingPlacer.StartPlacing(data);
        }

        private void PlaceBuilding(BuildingData data, Vector3 position)
        {
            Instantiate(data.prefab, position, Quaternion.identity);
            OnCraftSuccess(data);
        }
        protected override void OnCraftSuccess(BuildingData data)
        {
            base.OnCraftSuccess(data);
        }

        protected override void OnCraftFailure(BuildingData data)
        {
            base.OnCraftFailure(data);
        }
    }

}
