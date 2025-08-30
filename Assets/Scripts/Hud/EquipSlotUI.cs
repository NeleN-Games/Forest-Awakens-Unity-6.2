using Databases;
using Enums;
using Hud.Slots;
using Managers;
using Models.Data;
using Services;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Hud
{
    public class EquipSlotUI : MonoBehaviour, IDropHandler
    {
        public EquipSlotType slotType ;

        public Image icon;
        private object _equippedItem; 
        
        public void OnDrop(PointerEventData eventData)
        {
            Debug.Log("OnDrop called");

            var inventorySlot = eventData.pointerDrag.GetComponent<InventorySlotUI>();
            if (inventorySlot == null)
            {
                Debug.LogWarning("Dropped object does not have InventorySlotUI component");
                return;
            }

            var equippable = inventorySlot.GetEquippable();
            if (equippable == null)
            {
                Debug.LogWarning("InventorySlotUI does not contain a valid equippable item");
                return;
            }

            Debug.Log($"Dropped item: {equippable}, Type: {equippable.GetType()}");

            if (slotType == EquipSlotType.Consumable && equippable is SourceData source && source.isConsumable)
            {
                Debug.Log($"Equipping consumable: {source.name}");
                Equip(source, source.icon);
            }
            else if (slotType == EquipSlotType.Weapon && equippable is ItemData weapon)
            {
                Debug.Log($"Equipping weapon: {weapon.name}");
                Equip(weapon, weapon.icon);
            }
            else if (slotType == EquipSlotType.Gadget && equippable is ItemData gadget)
            {
                Debug.Log($"Equipping gadget: {gadget.name}");
                Equip(gadget, gadget.icon);
            }
            else
            {
                Debug.LogWarning($"Dropped item {equippable.GetName()} does not match the slot type {slotType}");
            }
        }


        public void Equip(object data, Sprite sprite)
        {
            _equippedItem = data;
            icon.sprite = sprite;
            icon.enabled = true;

            ServiceLocator.Get<EquipmentManager>().Equip(slotType, data);
        }

        public void Unequip()
        {
            _equippedItem = null;
            icon.sprite = null;
            icon.enabled = false;
        }
    }
}
