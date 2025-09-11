using UnityEngine;
using UnityEditor;

namespace Editor.Utilities.Themes
{
    public class MyCyberTheme : IEditorTheme
    {
        
        // Palette
        public static readonly Color BgColorStatic         = new Color(0.05f, 0.05f, 0.1f);  // Deep navy
        public static readonly Color FieldBorderColorStatic= new Color(0.1f, 0.1f, 0.2f);   // Soft darker border
        public static readonly Color LabelColorStatic      = new Color(0.9f, 0.95f, 1f);    // Soft cyan white

        public static readonly Color Color1Static          = new Color(0.0f, 1f, 1f);       // Neon cyan (titles)
        public static readonly Color Color2Static          = new Color(0.8f, 0.3f, 1f);     // Magenta (section headers)
        public static readonly Color Color3Static          = new Color(0.3f, 0.8f, 1f);     // Cyan glow (buttons)
        public static readonly Color Color4Static          = new Color(1f, 0.5f, 1f);       // Pink accents
        public static readonly Color Color5Static          = new Color(0.2f, 0.2f, 0.5f);   // Dark violet accent

        // Interface properties
        public Color BgColor => BgColorStatic;
        public Color FieldBorderColor => FieldBorderColorStatic;
        public Color LabelColor => LabelColorStatic;
        public Color Color1 => Color1Static;
        public Color Color2 => Color2Static;
        public Color Color3 => Color3Static;
        public Color Color4 => Color4Static;
        public Color Color5 => Color5Static;

        // Interface methods
        public GUIStyle TitleStyle() => TitleStyleStatic();
        public GUIStyle SectionHeaderStyle() => SectionHeaderStyleStatic();
        public GUIStyle LabelStyle() => LabelStyleStatic();
        public GUIStyle ButtonStyle(Color glowColor) => ButtonStyleStatic(glowColor);
        public GUIStyle DropdownStyle() => DropdownStyleStatic();

        // GUIStyles
        public static GUIStyle TitleStyleStatic(int fontSize = 24)
        {
            return new GUIStyle(EditorStyles.boldLabel) { fontSize = fontSize, alignment = TextAnchor.MiddleCenter, normal = { textColor = Color1Static } };
        }
        public static GUIStyle SectionHeaderStyleStatic(int fontSize = 16)
        {
            return new GUIStyle(EditorStyles.boldLabel) { fontSize = fontSize, alignment = TextAnchor.MiddleLeft, normal = { textColor = Color2Static } };
        }
        public static GUIStyle LabelStyleStatic()
        {
            return new GUIStyle(EditorStyles.label) { normal = { textColor = LabelColorStatic }, fontStyle = FontStyle.Bold };
        }
        public static GUIStyle ButtonStyleStatic(Color hoverColor)
        {
            return new GUIStyle(GUI.skin.button) { normal = { textColor = hoverColor }, hover = { textColor = hoverColor }, fontStyle = FontStyle.Bold };
        }
        public static GUIStyle DropdownStyleStatic()
        {
            return new GUIStyle(EditorStyles.boldLabel) { fontSize = 15, alignment = TextAnchor.MiddleLeft, normal = { textColor = Color4Static }, hover = { textColor = Color3Static }, active = { textColor = Color2Static } };
        }
    }
}