using System;
using System.Collections.Generic;
using Enums;
using Hud;
using Interfaces;
using Models;
using Models.Data;
using Services;
using UnityEngine;

namespace Managers
{
    public class EquipmentManager : MonoBehaviour, IInitializable
    {
        private ItemData _weapon1;
        private ItemData _weapon2;
        private ItemData _gadget;
        private SourceData _consumable; 
        
        [SerializeField] private EquipSlotUI weapon1Slot;
        [SerializeField] private EquipSlotUI weapon2Slot;
        [SerializeField] private EquipSlotUI gadgetSlot;
        [SerializeField] private EquipSlotUI consumableSlot;

        private bool _isWeapon1Active = false;
        private bool _isWeapon2Active = false;

        public GameObject cashedDragIcon;
        public void Initialize()
        {
        }

        public void OnDestroy()
        {
        }
        
        public void Equip(EquipSlotType slot, object data)
        {
            switch (slot)
            {
                case EquipSlotType.Weapon:
                    if (_weapon1 == null) _weapon1 = data as ItemData;
                    else _weapon2 = data as ItemData;
                    break;

                case EquipSlotType.Gadget:
                    _gadget = data as ItemData;
                    break;

                case EquipSlotType.Consumable:
                    _consumable = data as SourceData;
                    break;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) ToggleWeapon(ref _isWeapon1Active, _weapon1);
            if (Input.GetKeyDown(KeyCode.Alpha2)) ToggleWeapon(ref _isWeapon2Active, _weapon2);
            if (Input.GetKeyDown(KeyCode.Alpha3) && _gadget != null) UseGadget(_gadget);
            if (Input.GetKeyDown(KeyCode.Alpha4) && _consumable != null) UseConsumable(_consumable);
        }

        private void ToggleWeapon(ref bool isActive, ItemData weapon)
        {
            if (weapon == null) return;

            if (isActive) isActive = false;
            else
            {
                _isWeapon1Active = false;
                _isWeapon2Active = false;
                isActive = true;
            }
        }

        private void UseGadget(ItemData gadgetData)
        {
            Debug.Log("Using _gadget: " + gadgetData.name);
        }

        private void UseConsumable(SourceData consumableData)
        {
            var inventory = ServiceLocator.Get<PlayerInventory>();
            if (!inventory.TryUseSource(consumableData))
            {
                _consumable = null; // اسلات رو خالی کن
                return;
            }

            Debug.Log("Consumed: " + consumableData.name);
            ApplyModifiers(consumableData.modifiers);

            if (inventory.GetSourceAmount(consumableData.GetEnum()) <= 0)
            {
                _consumable = null;
                consumableSlot.Unequip();
            }
        }
        private void ApplyModifiers(List<StatModifier> modifiers)
        {
            foreach(var modifier in modifiers)
            {
                StatManager.Instance.ApplyModifier(modifier);
            }
        }
      
        
    }
}
