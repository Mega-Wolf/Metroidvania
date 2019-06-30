#if UNITY_EDITOR

using UnityEditor;
#endif

using System;
using UnityEngine;

[System.AttributeUsage(System.AttributeTargets.All, Inherited = true, AllowMultiple = false)]
public class EnumFlagAttribute : PropertyAttribute {
    public string name;

    public EnumFlagAttribute() { }

    public EnumFlagAttribute(string name) {
        this.name = name;
    }
}

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(EnumFlagAttribute), true)]
public class EnumFlagDrawer : PropertyDrawer {

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EnumFlagAttribute flagSettings = (EnumFlagAttribute)attribute;
        Enum targetEnum = (Enum)Enum.ToObject(fieldInfo.FieldType, property.intValue);

        string propName = flagSettings.name;
        if (string.IsNullOrEmpty(propName)) {
            propName = ObjectNames.NicifyVariableName(property.name);
        }

        EditorGUI.BeginProperty(position, label, property);
        Enum enumNew = EditorGUI.EnumFlagsField(position, propName, targetEnum);
        property.intValue = (int)Convert.ChangeType(enumNew, targetEnum.GetType());
        EditorGUI.EndProperty();
    }


}

#endif