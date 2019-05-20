// https://gist.github.com/LotteMakesStuff/d6a9a4944fc667e557083108606b7d22

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System;
using System.Reflection;
using NaughtyAttributes.Editor;

[AttributeUsage(AttributeTargets.Field)]
public class AutohookAttribute : PropertyAttribute {

    public enum AutohookMode {
        OnlySameLevel,
        AlsoChildren,
        AlsoParent,
        AllParents
    }

    public readonly AutohookMode Mode = AutohookMode.AlsoChildren;

    public AutohookAttribute() { }
    public AutohookAttribute(AutohookMode mode) {
        Mode = mode;
    }
}

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(AutohookAttribute))]
public class AutohookPropertyDrawer : UnityEditor.PropertyDrawer {

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        // First, lets attempt to find a valid component we could hook into this property
        var component = FindAutohookTarget(property);
        if (component != null) {
            // if we found something, AND the autohook is empty, lets slot it.
            // the reason were straight up looking for a target component is so we
            // can skip drawing the field if theres a valid autohook. 
            // this just looks a bit cleaner but isnt particularly safe. YMMV
            if (property.objectReferenceValue == null)
                property.objectReferenceValue = component;
            return;
        }

        // havent found one? lets just draw the default property field, let the user manually
        // hook something in.
        EditorGUI.PropertyField(position, property, label);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        // if theres a valid autohook target we skip drawing, so height is zeroed
        var component = FindAutohookTarget(property);
        if (component != null)
            return 0;

        // otherwise, return its default height (which should be the standard 16px unity usually uses)
        return base.GetPropertyHeight(property, label);
    }

    /// <summary>
    /// Takes a SerializedProperty and finds a local component that can be slotted into it.
    /// Local in this context means its a component attached to the same GameObject.
    /// This could easily be changed to use GetComponentInParent/GetComponentInChildren
    /// </summary>
    /// <param name="property"></param>
    /// <returns></returns>
    private Component FindAutohookTarget(SerializedProperty property) {
        var root = property.serializedObject;

        if (root.targetObject is Component) {
            // first, lets find the type of component were trying to autohook...
            var type = GetTypeFromProperty(property);

            if (type == null) {
                return null;
            }

            // ...then use GetComponent(type) to see if there is one on our object.
            var component = (Component)root.targetObject;

            Component ret = component.GetComponent(type);

            if (ret != null) {
                return ret;
            }

            AutohookAttribute autohookAttribute = PropertyUtility.GetAttribute<AutohookAttribute>(property);
            switch (autohookAttribute.Mode) {
                case AutohookAttribute.AutohookMode.AlsoChildren:
                    return component.GetComponentInChildren(type);
                case AutohookAttribute.AutohookMode.AlsoParent:
                    return component.GetComponentInParent(type);
                case AutohookAttribute.AutohookMode.AllParents: {
                        Transform t = component.transform.parent;
                        while (t != null) {
                            ret = t.GetComponent(type);
                            if (ret != null) {
                                return ret;
                            }
                            t = t.parent;
                        }
                        return null;
                    }
            }


            return ret;
        } else {
            Debug.Log("OH NO handle fails here better pls");
        }

        return null;
    }

    /// <summary>
    /// Uses reflection to get the type from a serialized property
    /// </summary>
    /// <param name="property"></param>
    /// <returns></returns>
    private static System.Type GetTypeFromProperty(SerializedProperty property) {
        // first, lets get the Type of component this serialized property is part of...
        var parentComponentType = property.serializedObject.targetObject.GetType();
        // ... then, using reflection well get the raw field info of the property this
        // SerializedProperty represents...


        FieldInfo fieldInfo;

        while (true) {
            fieldInfo = parentComponentType.GetField(property.propertyPath, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            // ... using that we can return the raw .net type!

            if (parentComponentType.BaseType == null || fieldInfo != null) {
                break;
            }
            parentComponentType = parentComponentType.BaseType;
        }

        return fieldInfo?.FieldType;
    }
}

#endif