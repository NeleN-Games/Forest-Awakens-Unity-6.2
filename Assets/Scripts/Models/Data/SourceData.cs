using System;
using System.Collections.Generic;
using Enums;
using Interfaces;
using UnityEditor;
using UnityEngine;

namespace Models.Data
{
    [CreateAssetMenu(menuName = "Data/Source")]
    [Serializable]
    public class SourceData : CommonAssetData<SourceType>,IEquippable
    {
        public bool isConsumable; 
        public List<StatModifier> modifiers;

        public override SourceType GetEnum() => enumType;
        public Sprite GetIcon() => icon;
        public string GetName() => name;
        private string relatedScriptTypeName;
        public override CommonAssetData<SourceType> Clone()
        {
            return Instantiate(this);
        }
        public void SetRelatedScript(Type relatedType)
        {
            if (!typeof(MonoBehaviour).IsAssignableFrom(relatedType))
            {
                Debug.LogError("Type must be a MonoBehaviour");
                return;
            }
            relatedScriptTypeName = relatedType.AssemblyQualifiedName;
        }

        public Type GetRelatedScriptType()
        {
            if (string.IsNullOrEmpty(relatedScriptTypeName)) return null;
            return Type.GetType(relatedScriptTypeName);
        }

        
    }
}
