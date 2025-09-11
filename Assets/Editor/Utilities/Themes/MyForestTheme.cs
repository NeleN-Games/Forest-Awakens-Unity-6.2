using UnityEditor;
using UnityEngine;

namespace Editor.Utilities.Themes
{
    public class MyForestTheme : IEditorTheme
    {
        // Palette
        public static readonly Color BgColorStatic         = new Color(0.05f, 0.08f, 0.06f);  // Dark forest green
        public static readonly Color FieldBorderColorStatic= new Color(0.12f, 0.25f, 0.15f); // Mossy border
        public static readonly Color LabelColorStatic      = new Color(0.9f, 1f, 0.9f);      // Soft light green

        public static readonly Color Color1Static          = new Color(0.3f, 1f, 0.6f);      // Bright green (titles)
        public static readonly Color Color2Static          = new Color(0.2f, 0.8f, 0.5f);    // Teal green (section headers)
        public static readonly Color Color3Static          = new Color(0.2f, 0.7f, 1f);      // Soft blue (button glow)
        public static readonly Color Color4Static          = new Color(0.5f, 1f, 0.7f);      // Light green accents
        public static readonly Color Color5Static          = new Color(0.1f, 0.3f, 0.2f);    // Dark moss accent

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
