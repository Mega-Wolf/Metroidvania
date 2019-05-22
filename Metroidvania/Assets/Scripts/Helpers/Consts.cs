using System.Collections.Generic;
using NaughtyAttributes.Editor;
using UnityEditor;
using UnityEngine;
using static Consts;

[CustomEditor(typeof(Consts))]
public class ConstsEditor : Editor {

    public static string PATH = "Assets/Data/SOs";

    private List<SerializedProperty> f_sps = new List<SerializedProperty>();

    private void OnEnable() {
        var fields = ReflectionUtility.GetAllFields(this.target, f => this.serializedObject.FindProperty(f.Name) != null);
        foreach (var field in fields) {
            SerializedProperty sp = serializedObject.FindProperty(field.Name);

            //TODO
            // if (field.FieldType.BaseType == typeof(UnityEngine.ScriptableObject)) {
            //     if (sp.objectReferenceValue == null) {
            //         ScriptableObject asset = ScriptableObject.CreateInstance(field.FieldType);
            //         string path = PATH + "/" + field.FieldType.Name + ".asset";
            //         AssetDatabase.CreateAsset(asset, path);
            //         AssetDatabase.SaveAssets();
            //         AssetDatabase.Refresh();
            //         AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            //         sp.objectReferenceValue = asset;
            //         sp.serializedObject.ApplyModifiedProperties();
            //     }
            // }
            f_sps.Add(sp);
        }
        f_sps.Sort((a, b) => string.Compare(a.name, b.name));
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();

        foreach (SerializedProperty sp in f_sps) {
            EditorGUILayout.PropertyField(sp, true);
        }

        serializedObject.ApplyModifiedProperties();
    }

}

public partial class Consts : Singleton<Consts> {

    public Player Player;

}