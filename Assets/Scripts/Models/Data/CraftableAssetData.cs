using System;
using System.Collections.Generic;
using Enums;
using Interfaces;
using UnityEngine;
using Unity.Collections;

namespace Models.Data
{
    public abstract class CraftableAssetData<TEnum> : CommonAssetData<TEnum>,ICraftable
        where TEnum : System.Enum
    {
        [SerializeField,ReadOnly]
        private UniqueId uniqueId;

        [SerializeField]
        private CategoryType categoryType;

        [SerializeField]
        private CraftableAvailabilityState craftableAvailabilityState;
        public UniqueId UniqueId { get;  set; }


        public CategoryType CategoryType
        {
            get => categoryType;
            set => categoryType = value;
        }

        public CraftableAvailabilityState CraftableAvailabilityState
        {
            get => craftableAvailabilityState;
            set => craftableAvailabilityState = value;
        }
        public virtual UniqueId GetUniqueId()=>UniqueId;
    
        public abstract void Craft();
        [SerializeField]
        private List<SourceRequirement> _resourceRequirements;
        public List<SourceRequirement> GetRequirements() => _resourceRequirements;
        public void SetRequirements(List<SourceRequirement> sourceRequirements)
        {
            _resourceRequirements=sourceRequirements;
        }
        
        public bool IsAvailabilityChanged(IInventoryService inventory)
        {
            var oldState = CraftableAvailabilityState;
            var newState = IsAvailable(inventory) 
                ? CraftableAvailabilityState.Available 
                : CraftableAvailabilityState.Unavailable;

            CraftableAvailabilityState = newState;
            return oldState != newState;
        }

        public bool IsAvailable(IInventoryService inventory)
        {
            foreach (var req in _resourceRequirements)
            {
                if (inventory.GetSourceAmount(req.sourceType) < req.amount)
                    return false;
            }
            return true;
        }


        public void Initialize(GameObject prefab, Sprite icon, TEnum enumType,
            List<SourceRequirement> resourceRequirements,CategoryType categoryType, UniqueId uniqueId, CraftableAvailabilityState craftableAvailabilityState)
        {
            base.Initialize(prefab, icon, enumType);
            this._resourceRequirements = resourceRequirements ?? _resourceRequirements;
            this.CategoryType =categoryType;
            this.UniqueId =uniqueId;
            this.CraftableAvailabilityState =craftableAvailabilityState;
        }

        protected override bool IsValid()
        {
            return base.IsValid() && _resourceRequirements != null && _resourceRequirements.Count > 0;
        }

        public override CommonAssetData<TEnum> Clone()
        {
            var clone = ScriptableObject.Instantiate(this) as CraftableAssetData<TEnum>;
            if (clone == null)
            {
                Debug.LogError("Failed to clone CraftableAssetData");
                return null;
            }
            clone.SetRequirements(new List<SourceRequirement>(this.GetRequirements()));
            clone.UniqueId = this.UniqueId;
            clone.CategoryType = this.CategoryType;
            clone.CraftableAvailabilityState = this.CraftableAvailabilityState;
            return clone;
            /*var clone = Instantiate(this);
            clone.SetRequirements(this.GetRequirements());
            if (clone is CraftableAssetData<BuildingType> craftableClone)
            {
                PrintDetails();
            }
            else
            {
                Debug.Log("Clone created but it's not CraftableAssetData");
            }
            return clone;*/
        }

        private void PrintDetails()
        {
            Debug.Log($"CategoryType: {CategoryType}");
            Debug.Log($"UniqueId: {UniqueId?.ID}");
            Debug.Log($"CraftableAvailabilityState: {CraftableAvailabilityState}");
            Debug.Log($"Resource Requirements Count: {GetRequirements()?.Count}");
        }
    }
}
