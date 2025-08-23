using System;
using System.Collections.Generic;
using System.Linq;
using Databases;
using Enums;
using Hud.Slots;
using Interfaces;
using Models;
using Models.Data;
using Services;
using UnityEngine;

namespace Managers
{
    public class CraftLookupManager : MonoBehaviour,IInitializable
    {       
        
        /// <summary>
        /// Invoked when the availability of craftable objects changes, such as:
        /// - A new item is discovered in the tech tree.
        /// - The required resources for crafting become insufficient or sufficient.
        /// </summary>
        public Action<SourceType> OnSourceAmountChanged;

        /// <summary>
        /// Invoked when the availability of craftable objects changes, such as:
        /// - A new item is discovered in the tech tree.
        /// </summary>
        //public Action<Dictionary<CategoryType, List<ICraftable>>> OnUnlockedCraftableObject;
        
        private readonly Dictionary<SourceType, List<ICraftable>> _craftablesByRequiredSource= new();

        private readonly Dictionary<CategoryType, List<ICraftable>> _unlockedCraftableByCategory = new();
        
        private readonly Dictionary<ICraftable, CraftableSlotUI> CraftableSlotByICraftable=new ();
        
        private readonly Dictionary<CategoryType, CategorySlotUI> _categorySlots = new();
        private readonly List<CraftableSlotUI> _craftableSlots = new();

        [SerializeField] private CategorySlotUI categorySlotPrefab;
        [SerializeField] private CraftableSlotUI craftableSlotPrefab; 
        
        [SerializeField] private Transform categoriesSlotParent;
        [SerializeField] private Transform craftableSlotsContainer;
        
        private int _maxCraftableCount = 0;
        
        private IInventoryService _inventory;

        public void Initialize()
        {
            _inventory = ServiceLocator.Get<PlayerInventory>();
            if (_inventory == null)
                Debug.LogError("Inventory Service is not registered!");
            OnSourceAmountChanged += UpdateAvailabilityBySource;
            //OnUnlockedCraftableObject += AddUnlockedCraftableObjects;
            InitializeDatabases();
        }

        public void OnDestroy()
        {
            OnSourceAmountChanged -= UpdateAvailabilityBySource;
           // OnUnlockedCraftableObject -= AddUnlockedCraftableObjects;
        }

        private void InitializeDatabases()
        {
            var itemDatabase=ServiceLocator.Get<ItemDatabase>();
            var buildingDatabase=ServiceLocator.Get<BuildingDatabase>();
            AddUnlockedEntries<ItemType, ItemData>(itemDatabase.Entries);
            AddUnlockedEntries<BuildingType, BuildingData>(buildingDatabase.Entries);
            InitializeCategorySlots();
        }
        private void AddUnlockedEntries<TEnum, TData>(List<TData> entries)
            where TEnum : Enum
            where TData : CraftableAssetData<TEnum>
        {

            foreach (var entry in entries)
            {
                if (entry.CraftableAvailabilityState == CraftableAvailabilityState.Locked) 
                    continue;
               
                var clonedEntry = (TData)entry.Clone();
                if (clonedEntry == null)
                {
                    Debug.LogError($"Clone failed for entry: {entry.enumType}");
                    continue;
                }
                if (!_unlockedCraftableByCategory.ContainsKey(clonedEntry.CategoryType))
                {
                    _unlockedCraftableByCategory[clonedEntry.CategoryType] = new List<ICraftable>();
                }
                _unlockedCraftableByCategory[clonedEntry.CategoryType].Add(clonedEntry);

                var resourceRequirements = clonedEntry.GetRequirements();
                foreach (var sourceRequirement in resourceRequirements)
                {
                    SourceType sourceType = sourceRequirement.sourceType;

                    if (!_craftablesByRequiredSource.ContainsKey(sourceType))
                    {
                        _craftablesByRequiredSource[sourceType] = new List<ICraftable>();
                    }
                    _craftablesByRequiredSource[sourceType].Add(clonedEntry);
                }
                
            }
            
        }

        private void AddUnlockedCraftableObjects(Dictionary<CategoryType, List<ICraftable>> unlockedCraftableObjects)
        {
            foreach (var categoryType in unlockedCraftableObjects.Keys)
            {
                if (!_unlockedCraftableByCategory.ContainsKey(categoryType))
                {
                    _unlockedCraftableByCategory[categoryType] = new List<ICraftable>();

                    CreateNewCategory(categoryType);
                    
                    //UpdateAvailableObjects(unlockedCraftableObjects);
                    // TODO : CREATE NEW CATEGORY SLOT
                }
                
                var iCraftables = unlockedCraftableObjects[categoryType];
                foreach (var craftable in iCraftables)
                {
                    if (craftable.CraftableAvailabilityState != CraftableAvailabilityState.Locked)
                    {
                        Debug.LogError($"This Craftable: {craftable} is unlocked already");
                        continue;
                    }
                    //CreateNewCraftable(craftable);
                    _unlockedCraftableByCategory[categoryType].Add(craftable);
                    UpdateCraftableAvailability(craftable);
                    // TODO : Check availability of craftable objects and set it .
                }
            }
        }

        private void CreateNewCategory(CategoryType categoryType)
        {
            if (_categorySlots.TryGetValue(categoryType, out var slotUI))
            {
                Debug.LogError("This category has already a CategorySlotUI");
                return;
            }
            
            var categorySlotUI = Instantiate(categorySlotPrefab, categoriesSlotParent);
            slotUI = categorySlotUI.GetComponent<CategorySlotUI>();
            
            if (slotUI == null)
            {
                Debug.LogError("CategorySlotPrefab is missing CategorySlotUI component!");
            }
            
            _categorySlots[categoryType] = slotUI;
            slotUI.Setup(categoryType);
            slotUI.OnClicked -= ShowCategoryCraftableObjects;
            slotUI.OnClicked += ShowCategoryCraftableObjects;
        }

      
        /*private void UpdateAvailableObjects(Dictionary<CategoryType, List<ICraftable>> availableCraftableObjects)
        {
            foreach (var kvp in availableCraftableObjects)
            {
                var category = kvp.Key;
                var craftableList = kvp.Value;
                _unlockedCraftableByCategory[category] = craftableList;
                if (!_categorySlots.TryGetValue(category, out var slotUI))
                {
                    var go = Instantiate(categorySlotPrefab, categoriesSlotParent);
                    slotUI = go.GetComponent<CategorySlotUI>();
                    if (slotUI == null)
                    {
                        Debug.LogError("CategorySlotPrefab is missing CategorySlotUI component!");
                        continue;
                    }

                    _categorySlots[category] = slotUI;
                }
                slotUI.Setup(category);
                slotUI.OnClicked -= ShowCategoryCraftableObjects;
                slotUI.OnClicked += ShowCategoryCraftableObjects;
            }
        }*/
        private void ShowCategoryCraftableObjects(CategorySlotUI clickedCategorySlot)
        {
            var category = clickedCategorySlot.Category;
            if (!_unlockedCraftableByCategory.TryGetValue(category, out var craftableList))
                return;

            int itemCount = craftableList.Count;

            if (itemCount > _maxCraftableCount)
            {
                int toCreate = itemCount - _maxCraftableCount;
                for (int i = 0; i < toCreate; i++)
                {
                    var go = Instantiate(craftableSlotPrefab.gameObject, craftableSlotsContainer);
                    var slot = go.GetComponent<CraftableSlotUI>();
                    _craftableSlots.Add(slot);
                    
                }
                _maxCraftableCount = itemCount;
            }
            for (int i = 0; i < _maxCraftableCount; i++)
            {
                if (i < itemCount)
                {
                    _craftableSlots[i].gameObject.SetActive(true);
                    _craftableSlots[i].Setup(craftableList[i]);
                    CraftableSlotByICraftable[craftableList[i]] = _craftableSlots[i];
                }
                else
                {
                    _craftableSlots[i].gameObject.SetActive(false);
                }
            }
        }
        private void UpdateAvailabilityBySource(SourceType sourceType)
        {
            if (!_craftablesByRequiredSource.TryGetValue(sourceType, out var affectedCraftables))
                return;

            foreach (var craftable in affectedCraftables)
            {         
                bool isAvailabilityChanged = craftable.IsAvailabilityChanged(_inventory);
                if (isAvailabilityChanged)
                {
                    ChangeCraftableSlotUIAvailability(craftable,craftable.CraftableAvailabilityState==CraftableAvailabilityState.Available);
                }
            }
        } 
        private void UpdateCraftableAvailability(ICraftable craftable)
        {
            bool isAvailabilityChanged = craftable.IsAvailabilityChanged(_inventory);
            if (isAvailabilityChanged)
            {
                ChangeCraftableSlotUIAvailability(craftable,craftable.CraftableAvailabilityState==CraftableAvailabilityState.Available);
            }
        }

        private void ChangeCraftableSlotUIAvailability(ICraftable craftable, bool isAvailable)
        {
            if (CraftableSlotByICraftable.ContainsKey(craftable))
            {
                CraftableSlotByICraftable[craftable].ChangeAvailability(isAvailable);
            }
            else
            {
                Debug.LogError($"{nameof(craftable)} is not added to {nameof(CraftableSlotByICraftable)}");
            }
        }

        private void InitializeCategorySlots()
        {
            for (int i = categoriesSlotParent.childCount-1; i >= 0; i--)
            {
                Destroy(categoriesSlotParent.GetChild(i).gameObject);
            }
            foreach (var kvp in _unlockedCraftableByCategory)
            {
                CategoryType category = kvp.Key;
                Debug.Log(category);
                CreateNewCategory(category);
            }
        }
        
        /*private void AddUnlockedEntries<TEnum, TData>(List<TData> entries)
            where TEnum : Enum
            where TData : CraftableAssetData<TEnum>,ICraftable
        {

            foreach (var entry in entries)
            {
                var uid = entry.GetUniqueId();
                
                if (uid != null && !_uniqueIdLookup.ContainsKey(uid))
                    _uniqueIdLookup[uid] = entry;

                if (!_categoryLookup.TryGetValue(entry.CategoryType, out var list))
                {
                    list = new List<ICraftable>();
                    _categoryLookup[entry.CategoryType] = list;
                }
                list.Add(entry);
            }

            UpdateAvailableCategoryLookup();
        }
        public ICraftable GetByUniqueId(UniqueId id) =>
            _uniqueIdLookup.GetValueOrDefault(id);

        public List<ICraftable> GetByCategory(CategoryType category) =>
            _categoryLookup.TryGetValue(category, out var list) ? list : new List<ICraftable>();

        private void UpdateAvailableCategoryLookup()
        {
            _availableCategoryLookup.Clear();

            foreach (var kvp in _categoryLookup)
            {
                var filteredList = kvp.Value
                    .FindAll(item => item.CraftableAvailabilityState  == CraftableAvailabilityState.Available
                                     || item.CraftableAvailabilityState == CraftableAvailabilityState.Unavailable);

                if (filteredList.Count > 0)
                    _availableCategoryLookup[kvp.Key] = filteredList;
            }
        }*/
    }
}
