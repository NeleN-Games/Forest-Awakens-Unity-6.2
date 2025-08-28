using Databases;
using Enums;
using Managers;
using Models.Data;
using Services;
using UnityEngine;

namespace Helper
{
    public class ItemCrafter : Crafter<ItemType, ItemData, ItemDatabase>
    {
        public void Awake()
        {
            Database = ServiceLocator.Get<ItemDatabase>();
            OnCraft -= Craft;
            OnCraft += Craft;
        }

        public void OnDestroy()
        {
            OnCraft -= Craft;
        }

        protected override void HandleCraft(ItemData data)
        {
            OnCraftSuccess(data);
        }

        protected override void OnCraftSuccess(ItemData data)
        {
            base.OnCraftSuccess(data);
        }

        protected override void OnCraftFailure(ItemData data)
        {
            base.OnCraftFailure(data);
        }
    }
}
