using System.Collections.Generic;
using Editor.Utilities.Themes;
using UnityEditor;
using UnityEngine;

namespace Editor.Utilities
{
    public static class EditorThemeController
    {
        // Registry of available themes
        public static List<IEditorTheme> Themes = new List<IEditorTheme>
        {
            new MyDefaultTheme(),
            new MyVioletTheme(),
            new MyCyberTheme(),
            new MySunsetTheme(),
            new MyForestTheme()
        };

        public static string[] ThemeNames = new string[] { "Default", "Violet","Cyber","Sunset" ,"Forest" };

        // Current selected theme index (private, managed internally)
        private static int selectedIndex = 0;
        
        // Public accessor for current theme
        public static IEditorTheme CurrentTheme => Themes[selectedIndex];

        // Load the last used theme from EditorPrefs
        public static void LoadTheme(string editorName)
        {
            selectedIndex = EditorPrefs.GetInt($"{editorName}_ThemeIndex", 0);
            selectedIndex = Mathf.Clamp(selectedIndex, 0, Themes.Count - 1);
        }

        // Save current theme to EditorPrefs
        public static void SaveTheme(string editorName)
        {
            EditorPrefs.SetInt($"{editorName}_ThemeIndex", selectedIndex);
        }

        // Draws the theme dropdown and handles selection internally
        public static void DrawThemeDropdown(string editorName)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Theme:", GUILayout.Width(50));

            // Draw popup and get new selection
            int currentIndex = Themes.IndexOf(CurrentTheme);
            int newIndex = EditorGUILayout.Popup(currentIndex, ThemeNames);

            if (newIndex != currentIndex)
            {
                selectedIndex = newIndex;
                SaveTheme(editorName);
            }


            EditorGUILayout.EndHorizontal();
        }
    }
}
