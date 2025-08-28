using System;
using System.Linq;
using Databases;
using Interfaces;
using Models;
using Models.Data;
using Services;
using UnityEngine;

namespace Managers
{
    public abstract class Crafter<TEnum, TData, TDatabase> : MonoBehaviour
        where TEnum : Enum
        where TData : CraftableAssetData<TEnum>
        where TDatabase : GenericDatabase<TEnum, TData>
    {
        public Action<CraftCommand<TEnum>> OnCraft;

        protected TDatabase Database;
        

        protected void Craft(CraftCommand<TEnum> command)
        {
            var data = Database.Get(command.ID);
            if (data == null)
            {
                OnCraftFailure(null);
                return;
            }

            if (ServiceLocator.Get<PlayerInventory>().HasEnoughSources(data.GetRequirements()))
            {
                HandleCraft(data);
                //OnCraftSuccess(data);
            }
            else
            {
                OnCraftFailure(data);
            }
        }


        protected abstract void HandleCraft(TData data);

        protected virtual void OnCraftSuccess(TData data)
        {
            Debug.Log($"{data.name} has been crafted.");
        }

        protected virtual void OnCraftFailure(TData data)
        {
            Debug.Log($"Failed to craft {data.name}.");

        }
      
    }
}