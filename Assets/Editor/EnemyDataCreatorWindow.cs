using DG.Tweening;
using Editor.Utilities;
using Enums;
using Interfaces.Enemy;
using Models.Data;
using Models.Data.Enemy;
using Models.Structs;
using ScriptGenerators;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class EnemyDataCreatorWindow : EditorWindow
    {
        private IEditorTheme currentTheme;
        private CustomMeshPreview _customMeshPreview;

        private const string EnemyPrefabPath = "Assets/Prefabs/Enemies";
        private const string EnemyScriptPath = "Assets/Scripts/Models/Enemies";
        private const string EnemyDataPath = "Assets/Resources/Enemies";

        private string enemyName = "NewEnemy";
        private float health = 100f;

        private EnemyCategory category = EnemyCategory.Attacker;
        private EnemySubCategory subCategory = EnemySubCategory.Melee;

        private Mesh selectedMesh;
        private Material selectedMaterial;
        private Sprite icon;

        private IEnemyBehavior.IIdleBehaviour idleBehavior;
        private IEnemyBehavior.IAwarenessBehaviour awareBehavior;
        private IEnemyBehavior.IActBehaviour actBehavior;

        private bool canCreate = false; // Whether Create button is active after Prepare

        private Vector2 scrollPos;

        private PreviewRenderUtility previewUtility;
        private bool showPreview = true;

        private System.Collections.Generic.List<IEnemyBehavior.IIdleBehaviour> idleBehaviors;
        private string[] idleBehaviorNames;
        private System.Collections.Generic.List<IEnemyBehavior.IAwarenessBehaviour> awareBehaviors;
        private string[] awareBehaviorNames;
        private System.Collections.Generic.List<IEnemyBehavior.IActBehaviour> actBehaviors;
        private string[] actBehaviorNames;


        // EditorPrefs keys
        private string EditorThemeKey => "EnemyDataCreator_ThemeIndex";
        private string EnemyNameKey => "EnemyName";
        private string HealthKey => "EnemyHealth";
        private string CategoryKey => "EnemyCategory";
        private string SubCategoryKey => "EnemySubCategory";
        private string MeshKey => "EnemyMesh";
        private string MaterialKey => "EnemyMaterial";
        private string IconKey => "EnemyIcon";
        private string IdleKey => "EnemyIdleBehavior";
        private string AwareKey => "EnemyAwareBehavior";
        private string ActKey => "EnemyActBehavior";

        [MenuItem("Tools/Enemy Data Creator")]
        public static void ShowWindow() => GetWindow<EnemyDataCreatorWindow>("💀 Dark Enemy Creator ☠️");


        private void OnEnable()
        {
            LoadEditorPrefs();
            _customMeshPreview = new CustomMeshPreview();
            CacheAllBehaviors();
        }

        private void OnDisable()
        {
            SaveEditorPrefs();
            _customMeshPreview.Cleanup();
        }

        private void OnGUI()
        {
            EditorGUI.DrawRect(new Rect(0, 0, position.width, position.height), currentTheme.BgColor);

            // Title
            DrawTitle("💀 Create Enemy Data ☠️");
            EditorGUILayout.Space(10);

            EditorThemeController.DrawThemeDropdown(EditorThemeKey);
            currentTheme = EditorThemeController.CurrentTheme;
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);


            // =============================
            // Section: Basic Info
            // =============================
            DrawSectionHeader("🧱 Basic Info");
            DrawFieldWithBorder("Enemy Name", () => { enemyName = EditorGUILayout.TextField(enemyName); });
            DrawFieldWithBorder("Health", () => { health = EditorGUILayout.FloatField(health); });
            DrawFieldWithBorder("Category", () => { category = (EnemyCategory)EditorGUILayout.EnumPopup(category,currentTheme.DropdownStyle()); });
            DrawFieldWithBorder("SubCategory",
                () => { subCategory = (EnemySubCategory)EditorGUILayout.EnumPopup(subCategory,currentTheme.DropdownStyle()); });
            DrawFieldWithBorder("Icon",
                () => { icon = (Sprite)EditorGUILayout.ObjectField(icon, typeof(Sprite), false); });

            EditorGUILayout.Space(8);

            // =============================
            // Section: Mesh & Material
            // =============================
            DrawSectionHeader("🎨 Mesh & Material");
            DrawMeshMaterialSection();

            EditorGUILayout.Space(15);

            // =============================
            // Section: Behaviors
            // =============================
            DrawSectionHeader("⚔️ Assign Behaviors");
            DrawFieldWithBorder("Idle Behavior",
                () => DrawBehaviorField(ref idleBehavior, idleBehaviors,
                    idleBehaviorNames));
            DrawFieldWithBorder("Awareness Behavior",
                () => DrawBehaviorField(ref awareBehavior, awareBehaviors,
                    awareBehaviorNames));
            DrawFieldWithBorder("Act Behavior",
                () => DrawBehaviorField(ref actBehavior, actBehaviors,
                    actBehaviorNames));

            EditorGUILayout.Space(12);

            // =============================
            // Buttons
            // =============================
            DrawButtonWithGlow("🔄 Refresh Behaviors", () => { CacheAllBehaviors(); }, currentTheme.Color3);
            EditorGUILayout.Space(8);

            DrawButtonWithGlow("🛠️ Prepare Create", () => { PrepareCreate(); }, currentTheme.Color3);
            EditorGUILayout.Space(8);

            GUI.enabled = canCreate;
            DrawButtonWithGlow("🔥 Create Enemy Data", () =>
            {
                CreateEnemyDataAsset();
                canCreate = false;
            }, new Color(1f, 0.2f, 0.2f));
            GUI.enabled = true;

            EditorGUILayout.EndScrollView();
        }

        private void DrawTitle(string text)
        {
            EditorGUILayout.LabelField(text, currentTheme.TitleStyle(), GUILayout.Height(35));
        }

        private void DrawSectionHeader(string text)
        {
            EditorGUILayout.LabelField(text, currentTheme.SectionHeaderStyle());
            EditorGUILayout.Space(4);
        }


        private void DrawFieldWithBorder(string label, System.Action drawField)
        {
            var rect = EditorGUILayout.BeginVertical("box");
            EditorGUI.DrawRect(rect, currentTheme.FieldBorderColor);
            EditorGUILayout.LabelField(label, currentTheme.LabelStyle());
            drawField();
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(3);
        }

        private void DrawButtonWithGlow(string text, System.Action onClick, Color glowColor)
        {
            if (GUILayout.Button(text, currentTheme.ButtonStyle(glowColor), GUILayout.Height(32))) onClick.Invoke();
        }

        private void DrawMeshMaterialSection()
        {
            showPreview = EditorGUILayout.ToggleLeft("👁 Show Mesh Preview", showPreview);

            if (showPreview)
            {
                var rect = EditorGUILayout.BeginHorizontal("box");
                EditorGUI.DrawRect(rect, currentTheme.BgColor);

                // Left side (fields)
                EditorGUILayout.BeginVertical(GUILayout.Width(220));
                DrawFieldWithBorder("Mesh",
                    () => { selectedMesh = (Mesh)EditorGUILayout.ObjectField(selectedMesh, typeof(Mesh), false); });
                DrawFieldWithBorder("Material",
                    () =>
                    {
                        selectedMaterial =
                            (Material)EditorGUILayout.ObjectField(selectedMaterial, typeof(Material), false);
                    });
                EditorGUILayout.EndVertical();

                // Right side (preview)
                EditorGUILayout.BeginVertical();
             
                if (selectedMesh != null && selectedMaterial != null)
                {
                    _customMeshPreview.SetMesh(selectedMesh, selectedMaterial);
                    Rect previewRect = GUILayoutUtility.GetRect(501, 250, GUILayout.ExpandWidth(true));
                    _customMeshPreview.DrawPreview(previewRect);
                }

                EditorGUILayout.EndVertical();

                EditorGUILayout.EndHorizontal();
            }
            else
            {
                DrawFieldWithBorder("Mesh",
                    () => { selectedMesh = (Mesh)EditorGUILayout.ObjectField(selectedMesh, typeof(Mesh), false); });
                DrawFieldWithBorder("Material",
                    () =>
                    {
                        selectedMaterial =
                            (Material)EditorGUILayout.ObjectField(selectedMaterial, typeof(Material), false);
                    });
            }
        }
        
        private void PrepareCreate()
        {
            if (string.IsNullOrEmpty(enemyName))
            {
                EditorUtility.DisplayDialog("Error", "Enemy Name cannot be empty!", "OK");
                return;
            }

            if (selectedMesh == null || selectedMaterial == null)
            {
                EditorUtility.DisplayDialog("Error", "Mesh and Material must be assigned!", "OK");
                return;
            }

            if (idleBehavior == null || awareBehavior == null || actBehavior == null)
            {
                EditorUtility.DisplayDialog("Error", "All Behaviors must be assigned!", "OK");
                return;
            }

            // Create Controller Script
            string scriptPath = EnemyScriptCreator.CreateControllerScript(enemyName);
            AssetDatabase.ImportAsset(scriptPath);
            AssetDatabase.Refresh();

            var message = $"Created  {enemyName}Controller in \n {scriptPath}";
            EditorUtility.DisplayDialog("Success", message, "Nice");
            canCreate = true;
        }


        private void CacheAllBehaviors()
        {
            CacheBehaviors(out idleBehaviors, out idleBehaviorNames);
            CacheBehaviors(out awareBehaviors, out awareBehaviorNames);
            CacheBehaviors(out actBehaviors, out actBehaviorNames);
        }

        private void CacheBehaviors<T>(out System.Collections.Generic.List<T> list, out string[] names) where T : class
        {
            list = new System.Collections.Generic.List<T>();
            var nameList = new System.Collections.Generic.List<string>();
            string[] guids = AssetDatabase.FindAssets("t:ScriptableObject");
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var obj = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
                if (obj is T b)
                {
                    list.Add(b);
                    nameList.Add(obj.name);
                }
            }

            names = nameList.ToArray();
        }

        private void DrawBehaviorField<T>(ref T behavior, System.Collections.Generic.List<T> list,
            string[] names) where T : class
        {
            int selectedIndex = behavior != null ? list.IndexOf(behavior) : -1;
            int newIndex = EditorGUILayout.Popup(selectedIndex, names,currentTheme.DropdownStyle());
            if (newIndex >= 0) behavior = list[newIndex];
        }


        private void CreateEnemyDataAsset()
        {
            string prefabPath = GetUniqueAssetPath($"{EnemyPrefabPath}/{enemyName}.prefab");
            string dataPath = GetUniqueAssetPath($"{EnemyDataPath}/{enemyName}.asset");

            // 1️⃣ Create temporary GameObject for Prefab
            GameObject go = new GameObject(enemyName);
            var filter = go.AddComponent<MeshFilter>();
            filter.mesh = selectedMesh;

            var renderer = go.AddComponent<MeshRenderer>();
            renderer.sharedMaterial = selectedMaterial;

            // 2️⃣ Add Controller
            string scriptPath = $"{EnemyScriptPath}/{enemyName}Controller.cs";
            MonoScript controllerScript = AssetDatabase.LoadAssetAtPath<MonoScript>(scriptPath);
            EnemyController controller = null;

            if (controllerScript != null)
            {
                var controllerClass = controllerScript.GetClass();
                if (controllerClass != null)
                {
                    var comp = go.AddComponent(controllerClass);
                    controller = comp as EnemyController;
                    if (controller == null)
                        Debug.LogError("Controller component is not of type EnemyController!");
                }
                else
                {
                    Debug.LogError("Controller class not found!");
                }
            }
            else
            {
                Debug.LogError($"Controller script not found at {scriptPath}");
            }

            // 3️⃣ Create Prefab from temporary GameObject
            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(go, prefabPath);
            DestroyImmediate(go); // GameObject موقت حذف شود

            // 4️⃣ Create EnemyData and assign Prefab
            EnemyData newData = null;
            switch (category)
            {
                case EnemyCategory.Attacker:
                    newData = CreateInstance<AttackerEnemyData>();
                    break;
                case EnemyCategory.Defender:
                    newData = CreateInstance<DefenderEnemyData>();
                    break;
                case EnemyCategory.Supporter:
                    newData = CreateInstance<SupporterEnemyData>();
                    break;
                case EnemyCategory.Escaper:
                    newData = CreateInstance<EscaperEnemyData>();
                    break;
                default:
                    Debug.LogError("Unknown category!");
                    return;
            }

            newData.Initialize(prefab, icon, health, new EnemyType(category, subCategory),
                idleBehavior, awareBehavior, actBehavior);

            // 5️⃣ Assign Data to Controller inside Prefab
            if (controller != null)
                controller.Data = newData;

            // 6️⃣ Save Asset
            AssetDatabase.CreateAsset(newData, dataPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            // 7️⃣ Select created Prefab
            Selection.activeObject = prefab;

            var message = "EnemyData and Prefab created!\n \n " +
                          $"Data: {dataPath}\n \nPrefab:  {prefabPath}\n \n " +
                          $"Behaviors: Idle = {(idleBehavior as ScriptableObject)?.name}\n \n " +
                          $"Aware = {(awareBehavior as ScriptableObject)?.name}\n \n " +
                          $"Act = {(actBehavior as ScriptableObject)?.name}";
            EditorUtility.DisplayDialog("Success", message, "Nice");
        }


        private string GetUniqueAssetPath(string path)
        {
            string uniquePath = path;
            int count = 1;
            while (System.IO.File.Exists(uniquePath))
            {
                uniquePath = System.IO.Path.GetDirectoryName(path) + "/" +
                             System.IO.Path.GetFileNameWithoutExtension(path) +
                             $"_{count}" + System.IO.Path.GetExtension(path);
                count++;
            }

            return uniquePath;
        }

        #region EditorPrefs Save/Load

        private void SaveEditorPrefs()
        {
            EditorPrefs.SetString(EnemyNameKey, enemyName);
            EditorPrefs.SetFloat(HealthKey, health);
            EditorPrefs.SetInt(CategoryKey, (int)category);
            EditorPrefs.SetInt(SubCategoryKey, (int)subCategory);

            SaveAssetGuid(MeshKey, selectedMesh);
            SaveAssetGuid(MaterialKey, selectedMaterial);
            SaveAssetGuid(IconKey, icon);
            SaveAssetGuid(IdleKey, idleBehavior as ScriptableObject);
            SaveAssetGuid(AwareKey, awareBehavior as ScriptableObject);
            SaveAssetGuid(ActKey, actBehavior as ScriptableObject);
            EditorThemeController.SaveTheme(EditorThemeKey);

        }

        private void LoadEditorPrefs()
        {
            enemyName = EditorPrefs.GetString(EnemyNameKey, "NewEnemy");
            health = EditorPrefs.GetFloat(HealthKey, 100f);
            category = (EnemyCategory)EditorPrefs.GetInt(CategoryKey, 0);
            subCategory = (EnemySubCategory)EditorPrefs.GetInt(SubCategoryKey, 0);

            selectedMesh = LoadAssetGuid<Mesh>(MeshKey);
            selectedMaterial = LoadAssetGuid<Material>(MaterialKey);
            icon = LoadAssetGuid<Sprite>(IconKey);

            idleBehavior = LoadAssetGuid<ScriptableObject>(IdleKey) as IEnemyBehavior.IIdleBehaviour;
            awareBehavior = LoadAssetGuid<ScriptableObject>(AwareKey) as IEnemyBehavior.IAwarenessBehaviour;
            actBehavior = LoadAssetGuid<ScriptableObject>(ActKey) as IEnemyBehavior.IActBehaviour;
            
            EditorThemeController.LoadTheme(EditorThemeKey);
            currentTheme=EditorThemeController.CurrentTheme;
        }

        private void SaveAssetGuid(string key, Object obj)
        {
            if (obj == null)
                EditorPrefs.DeleteKey(key);
            else
                EditorPrefs.SetString(key, AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(obj)));
        }

        private T LoadAssetGuid<T>(string key) where T : Object
        {
            string guid = EditorPrefs.GetString(key, "");
            if (string.IsNullOrEmpty(guid)) return null;
            return AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid));
        }

        #endregion

    }
}