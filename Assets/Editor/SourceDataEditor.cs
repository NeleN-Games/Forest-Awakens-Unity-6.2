// Assets/Editor/SourceDataEditor.cs
using Models.Data;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(SourceData))]
    public class SourceDataEditor : UnityEditor.Editor
    {
        SerializedProperty prefabProp;
        SerializedProperty iconProp;
        SerializedProperty enumTypeProp;
        SerializedProperty isConsumableProp;
        SerializedProperty modifiersProp;

        private bool modifiersFoldout = true;

        private void OnEnable()
        {
            prefabProp = serializedObject.FindProperty("prefab");
            iconProp = serializedObject.FindProperty("icon");
            enumTypeProp = serializedObject.FindProperty("enumType");
            isConsumableProp = serializedObject.FindProperty("isConsumable");
            modifiersProp = serializedObject.FindProperty("modifiers");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.LabelField("Source Data", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(prefabProp, new GUIContent("Prefab"));
            EditorGUILayout.PropertyField(iconProp, new GUIContent("Icon"));
            EditorGUILayout.PropertyField(enumTypeProp, new GUIContent("Enum Type"));

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(isConsumableProp, new GUIContent("Is Consumable"));

            EditorGUILayout.Space();

            EditorGUI.BeginDisabledGroup(!isConsumableProp.boolValue);
            EditorGUILayout.PropertyField(modifiersProp, true); // true => نمایش کامل لیست و children
            EditorGUI.EndDisabledGroup();

            if (!isConsumableProp.boolValue && modifiersProp.arraySize > 0)
            {
                EditorGUILayout.Space();
                if (GUILayout.Button("Clear Modifiers"))
                {
                    modifiersProp.ClearArray();
                    serializedObject.ApplyModifiedProperties();
                    EditorUtility.SetDirty(target);
                    serializedObject.Update();
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

    }
}
