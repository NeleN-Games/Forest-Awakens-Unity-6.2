using UnityEditor;
using UnityEngine;

namespace Editor.Utilities.Themes
{
    public class MyVioletTheme : IEditorTheme
    {
    
// =====================
// Dark Violet / Purple / Pink Theme (High Contrast, Vibrant)
// =====================
        public static readonly Color BgColorStatic        = new Color(0.10f, 0.05f, 0.18f); // Dark violet background
        public static readonly Color FieldBorderColorStatic = new Color(0.20f, 0.12f, 0.30f); // Slightly lighter violet border
        public static readonly Color LabelColorStatic     = new Color(0.95f, 0.85f, 1f); // Soft pinkish-white for labels

// Palette for titles, sections, buttons, accents
        public static readonly Color Color1Static         = new Color(0.8f, 0.4f, 1f); // Bright pink for Title (like your Color1 in Default)
        public static readonly Color Color2Static         = new Color(1f, 0.5f, 0.85f);  // Vibrant purple for Section Header
        public static readonly Color Color3Static         = new Color(0.9f, 0.6f, 1f);  // Pinkish glow for buttons
        public static readonly Color Color4Static         = new Color(0.7f, 0.6f, 0.9f); // Light purple accent (secondary highlights)
        public static readonly Color Color5Static         = new Color(0.4f, 0.3f, 0.6f); // Deep purple accent (subtle UI elements)
    
        
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
        
        public GUIStyle DropdownStyle()=>DropdownStyleStatic();
     

        // ===========================
        // Static GUIStyle methods
        // ===========================
        public static GUIStyle TitleStyleStatic(int fontSize = 24)
        {
            return new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = fontSize,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = Color1Static }
            };
        }

        public static GUIStyle SectionHeaderStyleStatic(int fontSize = 16)
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
                normal = { textColor = hoverColor },
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
                normal = { textColor = Color4Static },
                hover = {textColor = Color2Static},
                active = {textColor = Color4Static}

            };
        }
    }
}
