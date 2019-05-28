#if UNITY_EDITOR
using UnityEditor;
#endif

using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConstScriptableObject : ScriptableObject { }


#if UNITY_EDITOR

/// <summary>
/// Extends how ScriptableObject object references are displayed in the inspector
/// Shows you all values under the object reference
/// Also provides a button to create a new ScriptableObject if property is null.
/// </summary>
[CustomPropertyDrawer(typeof(ConstScriptableObject), true)]
public class ConstScriptableObjectDrawer : PropertyDrawer {

    private Color constColor = Color.magenta;
    private Color nonDefaultColor = Color.red;

    public static Dictionary<Type, ScriptableObject> f_defaultSOs = new Dictionary<Type, ScriptableObject>();
    public static Dictionary<ScriptableObject, ScriptableObject> f_backupSOs = new Dictionary<ScriptableObject, ScriptableObject>();

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
            SerializedObject serializedObjectOriginal = new SerializedObject(data);
            SerializedProperty prop = serializedObjectOriginal.GetIterator();
            if (prop.NextVisible(true)) {
                do {
                    if (prop.name == "m_Script") {
                        continue;
                    }
                    var subProp = serializedObjectOriginal.FindProperty(prop.name);
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
        if (!f_defaultSOs.TryGetValue(fieldInfo.FieldType, out defaultSO)) {
            defaultSO = UnityEditor.AssetDatabase.LoadAssetAtPath(ConstsEditor.PATH + "/" + fieldInfo.FieldType + ".asset", typeof(ScriptableObject)) as ScriptableObject;
            if (defaultSO == null) {
                defaultSO = ScriptableObject.CreateInstance(fieldInfo.FieldType);
                string path = ConstsEditor.PATH + "/" + fieldInfo.FieldType.Name + ".asset";
                AssetDatabase.CreateAsset(defaultSO, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            }
            f_defaultSOs[fieldInfo.FieldType] = defaultSO;
        }
        if (property.objectReferenceValue == null) {
            property.objectReferenceValue = defaultSO;
            property.serializedObject.ApplyModifiedProperties();
        }

        ScriptableObject originalSO = (ScriptableObject)property.objectReferenceValue;

        ScriptableObject backupSO;
        if (!f_backupSOs.TryGetValue((ScriptableObject)property.objectReferenceValue, out backupSO)) {
            string pathOriginal = AssetDatabase.GetAssetPath(property.objectReferenceValue);
            int pos = pathOriginal.LastIndexOf("/");
            string path = pathOriginal.Substring(0, pos) + "/Dynamic/" + pathOriginal.Substring(pos + 1);

            backupSO = UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(ScriptableObject)) as ScriptableObject;
            if (backupSO == null) {
                backupSO = ScriptableObject.CreateInstance(fieldInfo.FieldType);
                try {
                    AssetDatabase.CreateAsset(backupSO, path);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                    AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
                } catch { }
            }
            f_backupSOs[(ScriptableObject)property.objectReferenceValue] = backupSO;
        }

        EditorGUI.BeginProperty(position, label, property);
        if (property.objectReferenceValue != null) {

            bool modified = !originalSO.Equals(backupSO);

            property.isExpanded = EditorGUI.Foldout(new Rect(position.x, position.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight), property.isExpanded, property.displayName, true);
            Color dummyBackgroundColor = GUI.backgroundColor;
            if (property.objectReferenceValue == defaultSO) {
                GUI.backgroundColor = constColor;
            } else {
                GUI.backgroundColor = nonDefaultColor;
            }
            if (!modified) {
                GUI.backgroundColor /= 2;
            }

            //TODO; dont render when playing
            EditorGUI.PropertyField(new Rect(EditorGUIUtility.labelWidth + 14, position.y, position.width - EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight), property, GUIContent.none, true);
            GUI.backgroundColor = dummyBackgroundColor;
            if (GUI.changed) {
                property.serializedObject.ApplyModifiedProperties();
            }
            if (property.objectReferenceValue == null) {
                EditorGUIUtility.ExitGUI();
            }

            GUI.Button(new Rect(position.x + position.width - 20, position.y, 20, EditorGUIUtility.singleLineHeight), "R");
            GUI.Button(new Rect(position.x + position.width - 2 * 20, position.y, 20, EditorGUIUtility.singleLineHeight), "A");

            //TODO END

            if (property.isExpanded) {
                // Draw a background that shows us clearly which fields are part of the ScriptableObject
                Color dummyColor = GUI.color;
                if (property.objectReferenceValue == defaultSO) {
                    GUI.color = constColor / 3;
                } else {
                    GUI.color = nonDefaultColor / 3;
                }

                GUI.Box(new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing - 1, (position.width - 2 * 20f) * (2 / 3f), position.height - EditorGUIUtility.singleLineHeight - EditorGUIUtility.standardVerticalSpacing), "");
                GUI.color = Color.grey;
                GUI.Box(new Rect(position.x + (position.width - 2 * 20) * (2 / 3f), position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing - 1, (position.width - 2 * 20f) / 3f, position.height - EditorGUIUtility.singleLineHeight - EditorGUIUtility.standardVerticalSpacing), "");
                GUI.color = dummyColor;

                EditorGUI.indentLevel++;
                SerializedObject serializedObjectOriginal = new SerializedObject(originalSO);
                SerializedObject serializedObjectBackup = new SerializedObject(backupSO);

                // Iterate over all the values and draw them
                SerializedProperty propOriginal = serializedObjectOriginal.GetIterator();
                SerializedProperty propBackup = serializedObjectBackup.GetIterator();

                float y = position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                if (propOriginal.NextVisible(true)) {
                    propBackup.NextVisible(true);
                    do {
                        // Don't bother drawing the class file
                        if (propOriginal.name == "m_Script") {
                            continue;
                        }

                        float height = EditorGUI.GetPropertyHeight(propOriginal, new GUIContent(propOriginal.displayName), true);
                        EditorGUI.PropertyField(new Rect(position.x, y, (position.width - 2 * 20f) * 2 / 3f, height), propOriginal, true);
                        //EditorGUI.PropertyField(new Rect(position.x + (position.width - 2 * 20) * 2 / 3f, y, (position.width - 2 * 20) / 3, height), propBackup, GUIContent.none, true);

                        //TODO; List
                        if (propBackup.type == "vector" && propOriginal.isExpanded) {
                            //Vector
                            propBackup.Next(true);
                            //Array
                            propBackup.Next(true);

                            float yy = y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                            EditorGUI.PropertyField(new Rect(position.x + (position.width - 2 * 20) * 2 / 3f, yy, 100, EditorGUIUtility.singleLineHeight), propBackup, GUIContent.none, false);
                            //propBackup.intValue = EditorGUI.IntField(new Rect(position.x + (position.width - 2 * 20) * 2 / 3f, yy, 100, EditorGUIUtility.singleLineHeight), propBackup.intValue);

                            int amount = propBackup.intValue;

                            yy += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                            for (int i = 0; i < amount; ++i) {
                                propBackup.Next(true);
                                EditorGUI.PropertyField(new Rect(14 + position.x + (position.width - 2 * 20) * 2 / 3f, yy, 100, EditorGUIUtility.singleLineHeight), propBackup, GUIContent.none, true);
                                yy += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                            }
                        } else {
                            EditorGUI.PropertyField(new Rect(position.x + (position.width - 2 * 20) * 2 / 3f, y, (position.width - 2 * 20) / 3f, height), propBackup, GUIContent.none, false);
                        }

                        if (!SerializedProperty.DataEquals(propOriginal, propBackup)) {
                            if (GUI.Button(new Rect(position.x + position.width - 20, y, 20, height), "R")) {
                                serializedObjectOriginal.CopyFromSerializedProperty(propBackup);
                            }
                            if (GUI.Button(new Rect(position.x + position.width - 2 * 20, y, 20, height), "A")) {
                                serializedObjectBackup.CopyFromSerializedProperty(propOriginal);
                            }
                        }

                        y += height + EditorGUIUtility.standardVerticalSpacing;
                    }
                    while (propOriginal.NextVisible(false) | propBackup.NextVisible(false));
                }
                if (GUI.changed) {
                    serializedObjectOriginal.ApplyModifiedProperties();
                    serializedObjectBackup.ApplyModifiedProperties();
                }

                EditorGUI.indentLevel--;
            }
        } else {
            Debug.LogError("For some reason, this ConstScriptableObject does not have a reference.");
        }

        // not sure why this is done as well
        //property.serializedObject.ApplyModifiedProperties();

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

#endif