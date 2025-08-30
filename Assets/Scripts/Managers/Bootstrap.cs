using Databases;
using Helper;
using Hud;
using Services;
using UnityEngine;

namespace Managers
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private SourceDatabase sourceDatabase;
        [SerializeField] private ItemDatabase itemDatabase;
        [SerializeField] private BuildingDatabase buildingDatabase;
        [SerializeField] private CraftingUIManager craftingUIManager;
        [SerializeField] private PlayerInventory playerInventory;
        [SerializeField] private InventoryUI inventoryUI;
        [SerializeField] private CraftManager craftManager;
        [SerializeField] private EquipmentManager equipmentManager;

        private void Awake()
        {
            ServiceLocator.Register(sourceDatabase);
            ServiceLocator.Register(itemDatabase);
            ServiceLocator.Register(buildingDatabase);
            ServiceLocator.Register(inventoryUI);
            ServiceLocator.Register(craftingUIManager);
            ServiceLocator.Register(playerInventory);
            ServiceLocator.Register(craftManager);
            ServiceLocator.Register(equipmentManager);
            
            ServiceLocator.InitializeAll();
        }
    }
}