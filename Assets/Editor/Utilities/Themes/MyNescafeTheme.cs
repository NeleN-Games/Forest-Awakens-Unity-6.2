using UnityEditor;
using UnityEngine;

namespace Editor.Utilities.Themes
{
    public class MyNescafeTheme : IEditorTheme
    {
        // Palette
        public static readonly Color BgColorStatic         = new Color(0.15f, 0.09f, 0.05f); // Deep coffee brown background
        public static readonly Color FieldBorderColorStatic= new Color(0.35f, 0.22f, 0.12f); // Medium brown border
        public static readonly Color LabelColorStatic      = new Color(1f, 0.95f, 0.88f);     // Creamy light beige for labels

        public static readonly Color Color1Static          = new Color(0.87f, 0.58f, 0.33f);  // Latte-orange for titles
        public static readonly Color Color2Static          = new Color(0.7f, 0.4f, 0.2f);     // Rich brown for section headers
        public static readonly Color Color3Static          = new Color(0.9f, 0.6f, 0.4f);     // Warm caramel for button glow
        public static readonly Color Color4Static          = new Color(1f, 0.8f, 0.6f);       // Soft highlight accents
        public static readonly Color Color5Static          = new Color(0.4f, 0.25f, 0.15f);   // Dark coffee accent for subtle UI elements

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
