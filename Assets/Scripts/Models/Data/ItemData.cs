using System;
using System.Collections.Generic;
using Enums;
using Interfaces;
using UnityEngine;

namespace Models.Data
{
    [CreateAssetMenu(menuName = "Data/Item")]
    [Serializable]
    public class ItemData : CraftableAssetData<ItemType>, IEquippable
    {
       public override ItemType GetEnum() => enumType;
       public Sprite GetIcon() => icon;
       public string GetName() => name;
       public override void Craft()
       {
           Debug.Log("Crafting Item");
       }

      
    }
}