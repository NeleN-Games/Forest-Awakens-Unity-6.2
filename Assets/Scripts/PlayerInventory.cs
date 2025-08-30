using System;
using System.Collections.Generic;
using Databases;
using Enums;
using Hud;
using Hud.Slots;
using Interfaces;
using Managers;
using Models;
using Models.Data;
using Services;
using UnityEngine;

public class PlayerInventory : MonoBehaviour,IInitializable, IInventoryService
{
    public Action<Dictionary<InventorySlotInfo, InventorySlotUI>> OnInventoryChanged;

    public Action<SourceType> OnSourceAmountChanged;
    private readonly Dictionary<IEquippable, InventorySlotInfo> _inventory = new();
    private readonly Dictionary<SourceType, int> _inventoryBySource = new();
    public void Initialize()
    {
        OnInventoryChanged += ServiceLocator.Get<InventoryUI>().RefreshInventory;
        OnSourceAmountChanged += ServiceLocator.Get<CraftingUIManager>().OnSourceAmountChanged;
    }

    public void OnDestroy()
    {
        OnInventoryChanged -= ServiceLocator.Get<InventoryUI>().RefreshInventory;
        OnSourceAmountChanged -= ServiceLocator.Get<CraftingUIManager>().OnSourceAmountChanged;
    }
    public int GetObjectAmount(IEquippable equippable)
    {
        if (equippable == null) return 0;
        return _inventory.TryGetValue(equippable, out var info) ? info.Count : 0;
    }
    public int GetSourceAmount(SourceType sourceType)
    {
        return _inventoryBySource.TryGetValue(sourceType, out var amount) ? amount : 0;
    }

    public Dictionary<InventorySlotInfo, InventorySlotUI> GetInventory()
    {
        return BuildInventoryUIData();
    }

   
    public void AddItem(IEquippable equippable, int amount)
    {
        if (equippable == null || amount <= 0) return;

        if (!_inventory.TryGetValue(equippable, out var slotInfo))
        {
            slotInfo = new InventorySlotInfo { Equippable = equippable, Count = 0 };
            _inventory[equippable] = slotInfo;
        }

        slotInfo.Count += amount;
        if (equippable is SourceData sourceData)
        {
            var sourceType = sourceData.GetEnum();
            var current = _inventoryBySource.GetValueOrDefault(sourceType, 0);
            current += amount;
            _inventoryBySource[sourceType] = current;
            OnSourceAmountChanged?.Invoke(sourceType);
        }
        OnInventoryChanged?.Invoke(BuildInventoryUIData());
    }
    
    public void RemoveItem(IEquippable equippable, int amount)
    {
        if (equippable == null || amount <= 0) return;
        
        if (!_inventory.TryGetValue(equippable, out var slotInfo)) return;

        slotInfo.Count -= amount;

        if (slotInfo.Count <= 0)
        {
            _inventory.Remove(equippable);
        }
        
        if (equippable is SourceData sourceData)
        {
            var sourceType = sourceData.GetEnum();
            if (_inventoryBySource.TryGetValue(sourceType, out var current))
            {
                current -= amount;
                if (current <= 0)
                {
                    _inventoryBySource.Remove(sourceType);
                }
                else
                {
                    _inventoryBySource[sourceType] = current;
                }
                OnSourceAmountChanged?.Invoke(sourceType);
            }
        }
        
        OnInventoryChanged?.Invoke(BuildInventoryUIData());
    }
    
    public void AddSource(SourceType type,int amount)
    {
        if (amount <= 0) return;

        var current = _inventoryBySource.GetValueOrDefault(type, 0);
        current += amount;
        _inventoryBySource[type] = current;
        var inventoryEquippable = ServiceLocator.Get<SourceDatabase>().Get(type);
        if (inventoryEquippable != null)
        {
            if (!_inventory.TryGetValue(inventoryEquippable, out var slotInfo))
            {
                slotInfo = new InventorySlotInfo { Equippable = inventoryEquippable, Count = 0 };
                _inventory[inventoryEquippable] = slotInfo;
            }
            slotInfo.Count += amount;
            OnSourceAmountChanged?.Invoke(type);
        }

        OnInventoryChanged?.Invoke(BuildInventoryUIData());
    } 
    public void RemoveSource(SourceType type,int amount)
    {
        if (amount <= 0) return;

        if (!_inventoryBySource.TryGetValue(type, out var current) || current <= 0) return;
        
        current -= amount;
        if (current <= 0)
        {
            _inventoryBySource.Remove(type);
        }
        else
        {
            _inventoryBySource[type] = current;
        }
        
        var inventoryEquippable = ServiceLocator.Get<SourceDatabase>().Get(type);
        if (inventoryEquippable != null && _inventory.TryGetValue(inventoryEquippable, out var slotInfo))
        {
            slotInfo.Count -= amount;
            if (slotInfo.Count <= 0)
            {
                _inventory.Remove(inventoryEquippable);
            }
            OnSourceAmountChanged?.Invoke(type);
        }

        OnInventoryChanged?.Invoke(BuildInventoryUIData());
    }
 
    
    private Dictionary<InventorySlotInfo, InventorySlotUI> BuildInventoryUIData()
    {
        var result = new Dictionary<InventorySlotInfo, InventorySlotUI>();
        foreach (var kvp in _inventory)
        {
            result[kvp.Value] = null; 
        }
        return result;
    }

    public bool HasEnoughSources(List<SourceRequirement> sources)
    {
        foreach (var source in sources)
        {
            if ( _inventoryBySource[source.sourceType]>=source.amount )
            {
              continue;
            }
            return false;
        }
        return true;
    }
    public bool TryUseSource(SourceData consumableData)
    {
        if (consumableData == null) return false;

        var sourceType = consumableData.GetEnum();
        int currentAmount = GetSourceAmount(sourceType);

        if (currentAmount <= 0)
        {
            Debug.Log("No more " + consumableData.name + " left in inventory!");
            return false;
        }

        RemoveItem(consumableData, 1);

        if (GetSourceAmount(sourceType) <= 0)
        {
            Debug.Log(consumableData.name + " is finished!");
        }

        return true;
    }

}
