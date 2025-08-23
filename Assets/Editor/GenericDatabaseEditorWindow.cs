using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Base_Classes;
using Databases;
using Editor.Utilities;
using Enums;
using Interfaces;
using Managers;
using Models;
using Models.Data;
using ScriptGenerators;
using UnityEditor;
using UnityEngine;
using Utilities;
using Object = UnityEngine.Object;

namespace Editor
{
    public abstract class GenericDatabaseEditorWindow<TData, TDatabase, TEnum> : EditorWindow
        where TData :  CommonAssetData<TEnum>
        where TDatabase : GenericDatabase<TEnum,TData>
        where TEnum : struct, Enum
    {
    
        private string ScriptGeneratedKey => $"ScriptGenerated_{typeof(TEnum).Name}";
        private bool ScriptGenerated
        {
            get => EditorPrefs.GetBool(ScriptGeneratedKey, false);
            set => EditorPrefs.SetBool(ScriptGeneratedKey, value);
        }
        private string DisplayMeshKey => $"DisplayMesh_{typeof(TEnum).Name}";
        private Mesh SelectedMesh
        {
            get
            {
                string guid = EditorPrefs.GetString(DisplayMeshKey, "");
                if (string.IsNullOrEmpty(guid)) return null;
                string path = AssetDatabase.GUIDToAssetPath(guid);
                return AssetDatabase.LoadAssetAtPath<Mesh>(path);
            }
            set
            {
                if (value == null)
                {
                    EditorPrefs.DeleteKey(DisplayMeshKey);
                    return;
                }

                string path = AssetDatabase.GetAssetPath(value);
                string guid = AssetDatabase.AssetPathToGUID(path);
                EditorPrefs.SetString(DisplayMeshKey, guid);
            }
        }
        private string DisplayMaterialKey => $"SelectedMaterial_{typeof(TEnum).Name}";
        private Material SelectedMaterial
        {
            get
            {
                string guid = EditorPrefs.GetString(DisplayMaterialKey, "");
                if (string.IsNullOrEmpty(guid)) return null;
                string path = AssetDatabase.GUIDToAssetPath(guid);
                return AssetDatabase.LoadAssetAtPath<Material>(path);
            }
            set
            {
                if (value == null)
                {
                    EditorPrefs.DeleteKey(DisplayMaterialKey);
                    return;
                }

                string path = AssetDatabase.GetAssetPath(value);
                string guid = AssetDatabase.AssetPathToGUID(path);
                EditorPrefs.SetString(DisplayMaterialKey, guid);
            }
        }
        private string DisplaySpriteKey => $"DisplaySprite_{typeof(TEnum).Name}";
        private string DisplaySpriteIndexKey => $"DisplaySpriteIndex_{typeof(TEnum).Name}";
        private Sprite DisplaySprite
        {
            get
            {
                string guid = EditorPrefs.GetString(DisplaySpriteKey, "");
                int index = EditorPrefs.GetInt(DisplaySpriteIndexKey, -1);
                if (string.IsNullOrEmpty(guid)) return null;

                string path = AssetDatabase.GUIDToAssetPath(guid);

                Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(path).OfType<Sprite>().ToArray();
                if (sprites.Length == 0) return null;

                if (index >= 0 && index < sprites.Length)
                    return sprites[index];

                return sprites[0];
            }
            set
            {
                if (value == null)
                {
                    EditorPrefs.DeleteKey(DisplaySpriteKey);
                    EditorPrefs.DeleteKey(DisplaySpriteIndexKey);
                    return;
                }

                string path = AssetDatabase.GetAssetPath(value);
                string guid = AssetDatabase.AssetPathToGUID(path);

                Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(path).OfType<Sprite>().ToArray();
                int index = Array.IndexOf(sprites, value);

                EditorPrefs.SetString(DisplaySpriteKey, guid);
                EditorPrefs.SetInt(DisplaySpriteIndexKey, index);
            }
        }

        private TDatabase _database;
        private string EnumReadyKey => $"EnumReady_{typeof(TEnum).Name}";
        private bool EnumReady
        {
            get => EditorPrefs.GetBool(EnumReadyKey, false);
            set => EditorPrefs.SetBool(EnumReadyKey, value);
        }
        private const string CollectableTag = "Collectable";
        private int SelectedItemIndex { get; set; } = 0;
        private int  SelectedCategoryKey  => $"AssetName_SelectedCategory".GetHashCode();
        private CategoryType SelectedCategory
        {
            get
            {
                int storedValue = EditorPrefs.GetInt(SelectedCategoryKey.ToString());
                return (CategoryType)storedValue;
            }
            set => EditorPrefs.SetInt(SelectedCategoryKey.ToString(), (int)value);
        }
        private int  SelectedAvailabilityKey  => $"AssetName_SelectedAvailability ".GetHashCode();
        private CraftableAvailabilityState SelectedAvailability
        {
            get
            {
                int storedValue = EditorPrefs.GetInt(SelectedAvailabilityKey.ToString());
                return (CraftableAvailabilityState)storedValue;
            }
            set => EditorPrefs.SetInt(SelectedAvailabilityKey.ToString(), (int)value);
        }

        private string AssetNameKey => $"AssetName_{typeof(TEnum).Name}";
        private string AssetName
        {
            get => EditorPrefs.GetString(AssetNameKey, "");
            set => EditorPrefs.SetString(AssetNameKey, value);
        } 
        private string SourceAmountKey => $"SourceAmount{typeof(TEnum).Name}";
        private int SourceAmount
        {
            get => EditorPrefs.GetInt(SourceAmountKey, 0);
            set => EditorPrefs.SetInt(SourceAmountKey, value);
        }
        
        private List<SourceRequirement> _resourceRequirements = new();
        private bool RequiresResourceRequirements => SetRequiresResourceRequirements();
        protected abstract bool SetRequiresResourceRequirements();

        private string EditorName=> GetEditorName();
        protected abstract  string GetEditorName();
        /// <summary>
        /// Path to read/write database.
        /// </summary>
        private string DatabaseFolderPath => GetExpectedDatabasePath();   
        protected abstract  string GetExpectedDatabasePath();
        private string Database => GetExpectedDatabaseName();
        protected abstract string GetExpectedDatabaseName();
        
        /// <summary>
        /// Path to save created prefab.
        /// </summary>
        private string PrefabsFolderPath => GetExpectedPrefabsPath();
        protected abstract string GetExpectedPrefabsPath();
        
        /// <summary>
        /// Path to add/remove ScriptableObject like ItemData, SourceData, BuildingData.
        /// </summary>
        private string DataFolderPath => GetExpectedDataPath();
        protected abstract string GetExpectedDataPath();
        
        
        protected string EnumName => typeof(TEnum).Name;
        private string EnumPath =>GetEnumPath();
        protected abstract string GetEnumPath();
        public void Draw()
        {
            EditorGUILayout.BeginVertical("box");
            
            Drawer.DrawSectionHeader($"{EditorName} information", new Color(0.2f, 0.5f, 0.8f, 1f));
            
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.Space(10);

            EditorGUILayout.BeginVertical("box");
            DrawItemFields();
            DrawDatabaseField();
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.Space(10);

            EditorGUILayout.BeginVertical("box");
            
            Drawer.DrawSectionHeader($"{EditorName} Configuration", new Color(0.1f, 0.5f, 0.1f, 1f));
            
            EditorGUILayout.Space(10);
            
            if (RequiresResourceRequirements)
            {
                _resourceRequirements=SourceRequirementPrefs.Load();
                ResourceRequirementDrawer.Draw(ref _resourceRequirements);

                SelectedCategory = (CategoryType)EditorGUILayout.EnumPopup("Category", SelectedCategory);
                SelectedAvailability = (CraftableAvailabilityState)EditorGUILayout.EnumPopup("Availability", SelectedAvailability);

                EditorGUILayout.Space();
            }
            DrawEnumButton();
            
            EditorGUILayout.Space(10);
            
            if (!RequiresResourceRequirements)
            {
                DrawGenerateScriptButton();
                
                EditorGUILayout.Space(10);
            }
            
            DrawCreateButton();
            
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.Space(10);

            EditorGUILayout.BeginVertical("box");
            Drawer.DrawSectionHeader($"Delete Existing {EditorName}", new Color(0.8f, 0.1f, 0.1f, 1f));
                
            DrawDeleteSection();
            EditorGUILayout.EndVertical();
            
        }
        
        protected virtual void DrawItemFields()
        {
            EditorGUI.BeginChangeCheck();
            AssetName = StringUtility.CapitalizeFirstLetter(EditorGUILayout.TextField(GetNameFieldLabel(), AssetName));
            if (EditorGUI.EndChangeCheck())
            {
                EnumReady = false;
                ScriptGenerated = false;
            }
            EditorGUILayout.Space(5);
            EditorGUI.BeginChangeCheck();
            SelectedMesh = (Mesh)EditorGUILayout.ObjectField("Mesh", SelectedMesh, typeof(Mesh), false);
            if (EditorGUI.EndChangeCheck())
            {
                EnumReady = false;
                ScriptGenerated = false;
            }
            EditorGUILayout.Space(5);
            EditorGUI.BeginChangeCheck();
            SelectedMaterial = (Material)EditorGUILayout.ObjectField("Material", SelectedMaterial, typeof(Material), false);
            if (EditorGUI.EndChangeCheck())
            {
                EnumReady = false;
                ScriptGenerated = false;
            }
            EditorGUILayout.Space(5);
            EditorGUI.BeginChangeCheck();
            DisplaySprite = (Sprite)EditorGUILayout.ObjectField("Display Sprite", DisplaySprite, typeof(Sprite), false);
            if (EditorGUI.EndChangeCheck())
            {
                EnumReady = false;
                ScriptGenerated = false;
            }
            EditorGUILayout.Space(5);
            if (!RequiresResourceRequirements)
            {
                EditorGUI.BeginChangeCheck();
                SourceAmount = EditorGUILayout.IntField("Source Amount", SourceAmount);
                if (EditorGUI.EndChangeCheck())
                {
                    EnumReady = false;
                    ScriptGenerated = false;
                }
            }
           
        }
        protected abstract string GetNameFieldLabel();

        protected virtual void DrawDatabaseField()
        {
           
            EditorGUILayout.Space();
            _database = (TDatabase)EditorGUILayout.ObjectField($"{EditorName} Database", _database, typeof(GenericDatabase<TEnum,TData>), false);
        }

        protected virtual void DrawEnumButton()
        {
            if (GUILayout.Button("Check or Add Enum"))
            {
                if (string.IsNullOrWhiteSpace(AssetName))
                {
                    EditorUtility.DisplayDialog("Error", $"{EditorName} name cannot be empty.", "OK");
                    return;
                }

                if (Enum.TryParse<TEnum>(AssetName, out _))
                {
                    EditorUtility.DisplayDialog("Info", $"'{AssetName}' already exists in {EnumName} enum.", "OK");
                    EnumReady = false;
                    ScriptGenerated = false;
                }
                else
                {
                    SyncEnum();
                    ModifyEnumUtility.AddObjectTypeToEnum(AssetName,EnumPath,EnumName);
                    EnumReady = true;
                    EditorUtility.DisplayDialog("Added", $"'{AssetName}' was added to {EnumName} enum.\nYou can now create the {EditorName}.", "OK");
                }
            }
        }
        private void SyncEnum()
        {
            if (_database == null)
            {
                Debug.LogWarning("Database is null, cannot sync enum.");
                return;
            }
            EnumSyncUtility.SyncEnumFromDatabase<TEnum,TData,TDatabase>(EnumName, EnumPath, _database);

            AssetDatabase.Refresh();
        }
        
        protected virtual void DrawGenerateScriptButton()
        {
            var canCreateScript=EnumReady;
          
            GUI.enabled = canCreateScript;
            if (GUILayout.Button("Generate Source Script"))
            {
                if (string.IsNullOrWhiteSpace(AssetName))
                {
                    EditorUtility.DisplayDialog("Error", "Asset name cannot be empty.", "OK");
                    return;
                }

                if (!Enum.TryParse<TEnum>(AssetName, out _))
                {
                    SyncEnum();
                    ModifyEnumUtility.AddObjectTypeToEnum(AssetName, EnumPath, EnumName);
                    EditorUtility.DisplayDialog("Added", $"'{AssetName}' added to {EnumName} enum.", "OK");
                }

                SourceScriptGenerator.CreateCollectableScript(AssetName);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                ScriptGenerated = true;
                EditorUtility.DisplayDialog("Script Generated", $"Class '{AssetName}Source' generated.\nWait a moment for compilation before creating the item.", "OK");
            }
        }


        protected virtual void DrawCreateButton()
        {
            bool canCreate=false;
            if (RequiresResourceRequirements)
            {
                canCreate = EnumReady && _resourceRequirements.Count > 0;
            }
            else
            {
                canCreate = EnumReady && ScriptGenerated;
            }
            GUI.enabled = canCreate;
            var createBtnStyle = new GUIStyle(GUI.skin.button);
            if (canCreate) createBtnStyle.normal.textColor = Color.green;
            if (GUILayout.Button($"Create {EditorName}", createBtnStyle))
            {
                if (_database == null)
                {
                    EditorUtility.DisplayDialog("Missing Database", $"Please assign an {EditorName} database.", "OK");
                    return;
                }
                CreateItem();
                SyncEnum();
                EnumReady = false;
                ScriptGenerated = false;
                SourceRequirementPrefs.RemoveAll();
            }
            GUI.enabled = true;
        }
        private void CreateItem()
        {
            if (_database == null)
            {
                EditorUtility.DisplayDialog("Missing Database", $"Please assign an {EditorName} database.", "OK");
                return;
            }
            
            EnsureFolder(PrefabsFolderPath);
            EnsureFolder(DataFolderPath);
           
            Debug.Log(AssetName);
            GameObject itemObject = new GameObject(AssetName);
            if (SelectedMesh != null)
            {
                MeshFilter meshFilter = itemObject.AddComponent<MeshFilter>();
                meshFilter.sharedMesh = SelectedMesh;

                MeshRenderer meshRenderer = itemObject.AddComponent<MeshRenderer>();
                meshRenderer.sharedMaterial = SelectedMaterial != null ? SelectedMaterial : new Material(Shader.Find("Universal Render Pipeline/Lit"));
            }
            
            itemObject.AddComponent<BoxCollider>();
            
            // no anymore using 2d assets, use displaySprite to set in script for ui usage.
            //itemObject.AddComponent<SpriteRenderer>().sprite = DisplaySprite;
            string prefabPath = $"{PrefabsFolderPath}/{AssetName}.prefab";
            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(itemObject, prefabPath); 
            
            
            TData newItem = CreateInstance<TData>();
            
            newItem.name = AssetName;
            if (!Enum.TryParse(AssetName, out TEnum itemType))
            {
                Debug.LogError($"Enum.Parse failed: '{AssetName}' not found in {EnumName} after refresh.");
                return;
            }
            
            if (newItem is CraftableAssetData<TEnum> craftableData && RequiresResourceRequirements)
            {
                InitializeCraftableAsset(craftableData,prefab,itemType);
            }
            else
            {
                InitializeSourceAsset(prefab, newItem, itemType);
            }
            
            DestroyImmediate(itemObject);
            
            string assetPath = $"{DataFolderPath}/{AssetName}.asset";
            AssetDatabase.CreateAsset(newItem, assetPath);

            _database.Entries.Add(newItem);
            EditorUtility.SetDirty(_database);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.DisplayDialog("Success", $"Item '{AssetName}' created and added to database." +
                                                   $"\nItemData path is {assetPath}." +
                                                   $"\nPrefab path is {PrefabsFolderPath}", "Nice!");
        }
       private void EnsureFolder(string path)
        {
            var split = path.Split('/');
            string current = split[0];
            for (int i = 1; i < split.Length; i++)
            {
                string next = current + "/" + split[i];
                if (!AssetDatabase.IsValidFolder(next))
                    AssetDatabase.CreateFolder(current, split[i]);
                current = next;
            }
        }

        private void InitializeCraftableAsset(CraftableAssetData<TEnum> craftableData,GameObject prefab,TEnum itemType )
        {
            CraftableType craftableType;
            if (typeof(TEnum) == typeof(ItemType))
                craftableType = CraftableType.Item;
            else if (typeof(TEnum) == typeof(BuildingType))
                craftableType = CraftableType.Building;
            else
                throw new Exception($"Unsupported enum type {typeof(TEnum)}");

            var uniqueId = UniqueIdManager.CreateNewUniqueId(AssetName,craftableType);
            craftableData.SetRequirements(_resourceRequirements);
            craftableData.Initialize(prefab,DisplaySprite,itemType,_resourceRequirements,SelectedCategory,uniqueId,SelectedAvailability);
            craftableData.prefab = prefab;
            DatabasesManager.LoadDatabases();
            DatabasesManager.categoryDatabase.AddCraftableObjectToCategory(SelectedCategory,uniqueId);
            
        }

        private void InitializeSourceAsset(GameObject prefab,TData newItem,TEnum itemType)
        {
            prefab.tag = CollectableTag;
            newItem.Initialize(prefab,DisplaySprite,itemType);
            
            newItem.prefab = prefab;
            
            var collectableType = Type.GetType($"Models.Sources.{AssetName}Source, Assembly-CSharp");
            
            if (collectableType != null)
            {
                var component = prefab.AddComponent(collectableType);
                if (component is Collectable collectable)
                {
                    collectable.InitializeCollectable((SourceType)(object)itemType, SourceAmount);
                }
                if (newItem is SourceData sourceData)
                {
                    sourceData.SetRelatedScript(collectableType); 
                }
            }
            else
                Debug.LogError("Failed to find generated type. Make sure 'Generate Source Script' was clicked and compilation finished.");
        
        }
        protected virtual void DrawDeleteSection()
        {
            bool canDelete = _database != null && _database.Entries.Count > 0;
            GUI.enabled = canDelete;

            if (canDelete)
            {
                var itemNames = _database.Entries.Select(i => i.name).ToArray();
                SelectedItemIndex = EditorGUILayout.Popup("Select Item", SelectedItemIndex, itemNames);

                if (GUILayout.Button("Delete Selected Item") && IsValidIndex(SelectedItemIndex, itemNames.Length))
                {
                    var item = _database.Entries[SelectedItemIndex];
                    if (EditorUtility.DisplayDialog("Confirm Delete", $"Delete '{item.name}' and assets?", "Yes", "No"))
                    {
                        DeleteObjectAndAssets(item);
                        SyncEnum();
                        SelectedItemIndex = 0;
                    }
                }
            }
            else
            {
                EditorGUILayout.LabelField("No items found in the database.");
            }
            GUI.enabled = true;
         }
        private static bool IsValidIndex(int index, int length)
        {
            return index >= 0 && index < length;
        }private void DeleteScript(Type type)
        {
            if (type == null) return;

            string[] guids = AssetDatabase.FindAssets(type.Name + " t:MonoScript");
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                MonoScript ms = AssetDatabase.LoadAssetAtPath<MonoScript>(path);
                if (ms != null && ms.GetClass() == type)
                {
                    AssetDatabase.DeleteAsset(path);
                    Debug.Log($"Deleted script {type.Name} at {path}");
                    break;
                }
            }
        }

        private void DeleteObjectAndAssets(TData source)
        {
            if (_database.Remove(source))  
            {
                EditorUtility.SetDirty(_database);
              
                RemoveItemTypeFromEnum(source.name);
                CheckRemoveFromCategory(source);
                DeleteAssetIfExists(source.prefab);
                DeleteAssetIfExists(source);
                
                if (source is SourceData sourceData)
                {
                    DeleteScript(sourceData.GetRelatedScriptType());
                }
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                EditorUtility.DisplayDialog("Deleted", $"Selected {source.name} and its assets have been deleted.", "OK");
            }
            else
            {
                EditorUtility.DisplayDialog("Error", $"Failed to delete {source.name} from database.", "OK");
            }
        }
        private void DeleteAssetIfExists(UnityEngine.Object obj)
        {
            if (obj == null) return;
            string path = AssetDatabase.GetAssetPath(obj);
            if (!string.IsNullOrEmpty(path))
                AssetDatabase.DeleteAsset(path);
        }
        
        private void RemoveItemTypeFromEnum(string objectName)
        {
            string enumPath = EnumPath;
            if (!File.Exists(enumPath))
            {
                Debug.LogError("Enum file not found.");
                return;
            }

            var lines = File.ReadAllLines(enumPath).ToList();
            int enumStartIndex = lines.FindIndex(l => l.Contains($"enum {EnumName}"));
            if (enumStartIndex == -1)
            {
                Debug.LogError($"{EnumName} enum not found.");
                return;
            }

            int startIndex = enumStartIndex + 2;
            int endIndex = lines.FindIndex(startIndex, l => l.Trim().StartsWith("}"));
            if (endIndex == -1) endIndex = lines.Count;

            bool removed = false;
            for (int i = startIndex; i < endIndex; i++)
            {
                if (lines[i].Trim().TrimEnd(',').Trim() == objectName)
                {
                    lines.RemoveAt(i);
                    removed = true;
                    break;
                }
            }

            if (removed)
            {
                File.WriteAllLines(enumPath, lines);
                AssetDatabase.Refresh();
                Debug.Log($"Removed '{objectName}' from {EnumName} enum.");
            }
            else
            {
                Debug.LogWarning($"'{objectName}' not found in enum.");
            }
        }

        private void CheckRemoveFromCategory(TData source)
        {
            if (source is not CraftableAssetData<TEnum> craftableAssetData) return;
            
            DatabasesManager.LoadDatabases();
            DatabasesManager.categoryDatabase.RemoveCraftableObjectFromCategory(craftableAssetData.CategoryType,
                craftableAssetData.UniqueId);
            Debug.Log($"{source.name} removed from {craftableAssetData.CategoryType} category.");
        }
        protected virtual void OnEnable()
        {
            LoadAssets();
        }

        private void LoadAssets()
        {
            
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(TDatabase).Name}", new[] { DatabaseFolderPath });
            
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<TDatabase>(path);

                if (asset != null && asset.name == Database)
                {
                    _database = asset;
                    break;
                }
            }

            if (_database == null)
            {
                Debug.LogWarning($"Database not found: {Database} in folder {DatabaseFolderPath}");
            }
        }
    }
}
