using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class ConstScriptableObject : ScriptableObject { }

/// <summary>
/// Extends how ScriptableObject object references are displayed in the inspector
/// Shows you all values under the object reference
/// Also provides a button to create a new ScriptableObject if property is null.
/// </summary>
[CustomPropertyDrawer(typeof(ConstScriptableObject), true)]
public class ConstScriptableObjectDrawer : PropertyDrawer {

    private Color constColor = Color.magenta / 2;
    private Color nonDefaultColor = Color.red / 2;

    private static Dictionary<Type, ScriptableObject> f_constSOs = new Dictionary<Type, ScriptableObject>();

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        float totalHeight = EditorGUIUtility.singleLineHeight;

        //TODO check that sometime in the future
        // if (!IsThereAnyVisibileProperty(property)) {
        //     return totalHeight;
        // }

        if (property.isExpanded) {
            var data = property.objectReferenceValue as ScriptableObject;
            if (data == null) {
                return EditorGUIUtility.singleLineHeight;
            }
            SerializedObject serializedObject = new SerializedObject(data);
            SerializedProperty prop = serializedObject.GetIterator();
            if (prop.NextVisible(true)) {
                do {
                    if (prop.name == "m_Script") {
                        continue;
                    }
                    var subProp = serializedObject.FindProperty(prop.name);
                    float height = EditorGUI.GetPropertyHeight(subProp, null, true) + EditorGUIUtility.standardVerticalSpacing;
                    totalHeight += height;
                }
                while (prop.NextVisible(false));
            }
            // Add a tiny bit of height if open for the background
            totalHeight += EditorGUIUtility.standardVerticalSpacing;
        }
        return totalHeight;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

        ScriptableObject defaultSO;
        if (!f_constSOs.TryGetValue(fieldInfo.FieldType, out defaultSO)) {
            defaultSO = UnityEditor.AssetDatabase.LoadAssetAtPath(ConstsEditor.PATH + "/" + fieldInfo.FieldType + ".asset", typeof(ScriptableObject)) as ScriptableObject;
            if (defaultSO == null) {
                defaultSO = ScriptableObject.CreateInstance(fieldInfo.FieldType);
                string path = ConstsEditor.PATH + "/" + fieldInfo.FieldType.Name + ".asset";
                AssetDatabase.CreateAsset(defaultSO, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            }
            f_constSOs[fieldInfo.FieldType] = defaultSO;
            if (property.serializedObject == null) {
                property.objectReferenceValue = defaultSO;
                property.serializedObject.ApplyModifiedProperties();
            }
        }

        EditorGUI.BeginProperty(position, label, property);
        if (property.objectReferenceValue != null) {
            property.isExpanded = EditorGUI.Foldout(new Rect(position.x, position.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight), property.isExpanded, property.displayName, true);
            Color dummyBackgroundColor = GUI.backgroundColor;
            if (property.objectReferenceValue == defaultSO) {
                GUI.backgroundColor = constColor;
            } else {
                GUI.backgroundColor = nonDefaultColor;
            }

            EditorGUI.PropertyField(new Rect(EditorGUIUtility.labelWidth + 14, position.y, position.width - EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight), property, GUIContent.none, true);
            GUI.backgroundColor = dummyBackgroundColor;

            if (GUI.changed) {
                property.serializedObject.ApplyModifiedProperties();
            }
            if (property.objectReferenceValue == null) {
                EditorGUIUtility.ExitGUI();
            }

            if (property.isExpanded) {
                // Draw a background that shows us clearly which fields are part of the ScriptableObject
                Color dummyColor = GUI.color;
                if (property.objectReferenceValue == defaultSO) {
                    GUI.color = constColor / 1.5f;
                } else {
                    GUI.color = nonDefaultColor / 1.5f;
                }
                GUI.Box(new Rect(0, position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing - 1, Screen.width, position.height - EditorGUIUtility.singleLineHeight - EditorGUIUtility.standardVerticalSpacing), "");
                GUI.color = dummyColor;

                EditorGUI.indentLevel++;
                var data = (ScriptableObject)property.objectReferenceValue;
                SerializedObject serializedObject = new SerializedObject(data);

                // Iterate over all the values and draw them
                SerializedProperty prop = serializedObject.GetIterator();
                float y = position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                if (prop.NextVisible(true)) {
                    do {
                        // Don't bother drawing the class file
                        if (prop.name == "m_Script") {
                            continue;
                        }
                        float height = EditorGUI.GetPropertyHeight(prop, new GUIContent(prop.displayName), true);
                        EditorGUI.PropertyField(new Rect(position.x, y, position.width, height), prop, true);
                        y += height + EditorGUIUtility.standardVerticalSpacing;
                    }
                    while (prop.NextVisible(false));
                }
                if (GUI.changed) {
                    serializedObject.ApplyModifiedProperties();
                }

                EditorGUI.indentLevel--;
            }
        } else {
            Debug.LogError("For some reason, this ConstScriptableObject does not have a reference.");
        }
        property.serializedObject.ApplyModifiedProperties();
        EditorGUI.EndProperty();
    }

    //Creates a new ScriptableObject via the default Save File panel
    private ScriptableObject CreateAssetWithSavePrompt(Type type, string path) {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>)) {
            type = type.GetGenericArguments()[0]; // use this...
        }

        path = EditorUtility.SaveFilePanelInProject("Save ScriptableObject", "New " + type.Name + ".asset", "asset", "Enter a file name for the ScriptableObject.", path);
        if (path == "") {
            return null;
        }
        ScriptableObject asset = ScriptableObject.CreateInstance(type);
        AssetDatabase.CreateAsset(asset, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        EditorGUIUtility.PingObject(asset);
        return asset;
    }

    private bool IsThereAnyVisibileProperty(SerializedProperty property) {
        var data = (ScriptableObject)property.objectReferenceValue;
        SerializedObject serializedObject = new SerializedObject(data);

        SerializedProperty prop = serializedObject.GetIterator();

        while (prop.NextVisible(true)) {
            if (prop.name == "m_Script") {
                continue;
            }
            return true; //if theres any visible property other than m_script
        }
        return false;
    }
}