using UnityEditor;
using System.Diagnostics;
using System.IO;

public static class GitExtensionsProjectContextMenu
{
    [MenuItem("Assets/Open with GitExtensions ", false, 2000)]
    private static void OpenGitExtensionsHere()
    {
        string path = GetSelectedPath();

        if (string.IsNullOrEmpty(path))
        {
            UnityEngine.Debug.LogWarning("No valid path selected.");
            return;
        }

        if (File.Exists(path))
            path = Path.GetDirectoryName(path);

        string gitExtPath = @"C:\Program Files (x86)\GitExtensions\GitExtensions.exe";

        if (!File.Exists(gitExtPath))
        {
            UnityEngine.Debug.LogError("GitExtensions.exe not found! Update path in script.");
            return;
        }

        ProcessStartInfo psi = new ProcessStartInfo()
        {
            FileName = gitExtPath,
            Arguments = $"browse \"{path}\"",
            UseShellExecute = true
        };
        Process.Start(psi);
    }

    private static string GetSelectedPath()
    {
        string path = "Assets";

        if (Selection.assetGUIDs.Length == 0)
            return Path.GetFullPath(path);

        string selectedPath = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]);

        if (File.Exists(selectedPath))
            selectedPath = Path.GetDirectoryName(selectedPath);

        return Path.GetFullPath(selectedPath);
    }
}