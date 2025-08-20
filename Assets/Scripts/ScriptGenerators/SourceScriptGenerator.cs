using System.IO;
using UnityEditor;
using UnityEngine;

namespace ScriptGenerators
{
    public static class SourceScriptGenerator
    {
        public static void CreateCollectableScript(string assetName)
        {
            string scriptPath = $"Assets/Scripts/Models/Sources/{assetName}Source.cs";

            if (File.Exists(scriptPath)) return;

            string content = $@"using UnityEngine;
using Base_Classes;
using Enums;
namespace Models.Sources
{{
    public class {assetName}Source : Collectable
    {{
          
        public override void OnCollect(GameObject collector)
        {{
            base.OnCollect(collector);
        }}

        protected override void Awake()
        {{
            base.Awake();
        }}

        protected override void OnEnable()
        {{
            base.OnEnable();
        }}

        protected override void OnDisable()
        {{
            base.OnDisable();
        }}
    }}
}}";

            string folder = Path.GetDirectoryName(scriptPath);
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            File.WriteAllText(scriptPath, content);
            AssetDatabase.Refresh();
            Debug.Log($"{assetName}.cs script created at {scriptPath}");
        }
    }
}