using System.IO;
using UnityEditor;

namespace ScriptGenerators
{
    public static class EnemyScriptCreator
    {
        public static string CreateControllerScript(string enemyName)
        {
            string folder = "Assets/Scripts/Models/Enemies";
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            string className = $"{enemyName}Controller";
            string path = Path.Combine(folder, $"{className}.cs");

            if (File.Exists(path)) return path; // اگر قبلاً ساخته شده بود

            string template = $@"
using Enums;
using Models.Data.Enemy;
using UnityEngine;

namespace Models.Enemies
{{
    public class {className} : EnemyController
    {{
      
    }}
}}
";
            File.WriteAllText(path, template);
            AssetDatabase.ImportAsset(path);
            return path;
        }
    }
}