using UnityEngine;
using UnityEditor;

namespace Editor.Utilities
{
    public interface IEditorTheme
    {
        // Core colors
        Color BgColor { get; }
        Color FieldBorderColor { get; }
        Color LabelColor { get; }

        // Optional palette colors
        Color Color1 { get; }
        Color Color2 { get; }
        Color Color3 { get; }
        Color Color4 { get; }
        Color Color5 { get; }

        // Styles
        GUIStyle TitleStyle();
        GUIStyle SectionHeaderStyle();
        GUIStyle LabelStyle();
        GUIStyle ButtonStyle(Color glowColor);
        GUIStyle DropdownStyle();
        
    }
}