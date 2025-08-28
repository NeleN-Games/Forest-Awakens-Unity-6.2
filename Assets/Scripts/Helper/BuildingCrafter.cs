using Databases;
using Enums;
using Managers;
using Models.Data;
using Services;

namespace Helper
{
    public class BuildingCrafter : Crafter<BuildingType, BuildingData, BuildingDatabase>
    {
        public void Awake()
        {
            Database = ServiceLocator.Get<BuildingDatabase>();
            OnCraft -= Craft;
            OnCraft += Craft;
        }

        public void OnDestroy()
        {
            OnCraft -= Craft;
        }

        protected override void HandleCraft(BuildingData data)
        {
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
