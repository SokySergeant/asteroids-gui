using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Asteroids;
using UnityEngine;

[CustomEditor(typeof(GameData))]
public class AsteroidEditor : Editor
{
    // public override VisualElement CreatePropertyGUI(SerializedProperty property)
    // {
    //     return new PropertyField(property);
    //     
    //     var root = new VisualElement();
    //     
    //     root.Add(new Label("Asteroid"));
    //     root.Add(new PropertyField(property.FindPropertyRelative("asteroids")));
    //     
    //     return root;
    // }
    
    // public override float GetPropertyHeight (SerializedProperty property, GUIContent label){
    //     return 64;
    // }

    // private void OnEnable()
    // {
    //     
    // }
    //
    // public override void OnInspectorGUI() {
    //     DrawDefaultInspector();
    //
    //     GameData obj = (GameData)target;
    //
    //     obj.asteroids[0].minForce = EditorGUILayout.FloatField("Min Force", obj.asteroids[0].minForce);
    //
    //
    //
    //     EditorUtility.SetDirty(obj);
    // }
    
    // private SerializedProperty offset;
    //
    // private void OnEnable()
    // {
    //     // Link the properties
    //     offset = serializedObject.FindProperty("Offset");
    // }
    //
    // public override void OnInspectorGUI() 
    // {
    //     DrawDefaultInspector();
    //
    //     // Load the real class values into the serialized copy
    //     serializedObject.Update();
    //
    //     // Automatically uses the according PropertyDrawer for the type
    //     EditorGUILayout.PropertyField(offset);
    //
    //     // Write back changed values and evtl mark as dirty and handle undo/redo
    //     serializedObject.ApplyModifiedProperties();
    // }
    
}
