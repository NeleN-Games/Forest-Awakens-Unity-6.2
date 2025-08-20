using System.Collections.Generic;
using Databases;
using Editor.Utilities;
using Enums;
using Models;
using Models.Data;
using UnityEditor;


namespace Editor
{
    public class ItemCreatorWindow : GenericDatabaseEditorWindow<ItemData,ItemDatabase,ItemType>
    {
        protected override bool SetRequiresResourceRequirements()
        {
            // need to set Requirements
            return true;
        }

        protected override string GetEditorName()
        {
            return "Item";
        }

        protected override string GetExpectedDatabasePath()
        {
            return "Assets/Resources/Databases";
        }

        protected override string GetExpectedPrefabsPath()
        {
            return "Assets/Prefabs/Items";
        }
        
        protected override string GetExpectedDatabaseName()
        {
            return "Item Database";
        }
        protected override string GetExpectedDataPath()
        {
            return "Assets/Resources/Items";
        }

        protected override string GetEnumPath()
        {
            return $"Assets/Scripts/Enums/{EnumName}.cs";
        }
        
        protected override string GetNameFieldLabel()
        {
           return "Item Name";
        }

     }
}
