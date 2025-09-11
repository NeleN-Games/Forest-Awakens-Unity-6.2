using UnityEditor;
using UnityEngine;

namespace Editor.Utilities.Themes
{
    public class MyDefaultTheme : IEditorTheme
    {
        // Palette
        public static readonly Color BgColorStatic = new Color(0.08f, 0.08f, 0.1f);
        public static readonly Color FieldBorderColorStatic = new Color(0.12f, 0.12f, 0.15f);
        public static readonly Color LabelColorStatic = Color.white;
        public static readonly Color Color1Static = new Color(1f, 0.4f, 0.2f); // Title
        public static readonly Color Color2Static = new Color(0.9f, 0.3f, 0.1f); // Section Header
        public static readonly Color Color3Static = new Color(1f, 0.5f, 0f); // Button Glow
        public static readonly Color Color4Static = new Color(0.6f, 0.6f, 0.6f); // Optional accent
        public static readonly Color Color5Static = new Color(0.3f, 0.3f, 0.3f); // Optional accent

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

        // ===========================
        // Static GUIStyle methods
        // ===========================
        public static GUIStyle TitleStyleStatic(int fontSize = 22)
        {
            return new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = fontSize,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = Color1Static }
            };
        }

        public static GUIStyle SectionHeaderStyleStatic(int fontSize = 15)
        {
            return new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = fontSize,
                alignment = TextAnchor.MiddleLeft,
                normal = { textColor = Color2Static }
            };
        }

        public static GUIStyle LabelStyleStatic()
        {
            return new GUIStyle(EditorStyles.label)
            {
                normal = { textColor = LabelColorStatic },
                fontStyle = FontStyle.Bold
            };
        }

        public static GUIStyle ButtonStyleStatic(Color hoverColor)
        {
            return new GUIStyle(GUI.skin.button)
            {
                normal = { textColor = LabelColorStatic },
                hover = { textColor = hoverColor },
                fontStyle = FontStyle.Bold
            };
        }
        public static GUIStyle DropdownStyleStatic()
        {
            return new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 15,
                alignment = TextAnchor.MiddleLeft,
                normal = { textColor = Color1Static },
                hover = {textColor = Color3Static},
                active = {textColor = Color1Static}

            };
        }
    }
}
