using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WorldGenerator))]

public class LM_Inspector_Customizer : Editor {

    //public override void OnInspectorGUI()
    //{
    //    DrawDefaultInspector();

    //    SerializedProperty s_block_prefab;
    //    SerializedProperty s_block_positions;

    //    void OnEnable()
    //    {
    //        // Fetch the objects from the MyScript script to display in the inspector
    //        m_IntProperty = serializedObject.FindProperty("myInt");
    //    }
    //    //serializedObject.Update();
    //    //EditorGUILayout.PropertyField(serializedObject.FindProperty("Blocks_prefab"), true);
    //    //serializedObject.ApplyModifiedProperties();
    //}


    SerializedProperty s_block_prefab;
    SerializedProperty s_block_positions;
    //void OnEnable()
    //{
    //    // Fetch the objects from the MyScript script to display in the inspector
    //    //s_block_prefab = serializedObject.FindProperty("Blocks_prefabs");
    //    //s_block_positions = serializedObject.FindProperty("block_positions");
    //}

    //public override void OnInspectorGUI()
    //{
        //serializedObject.Update();
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("Blocks_prefabs"), true);
        ////EditorGUILayout.PropertyField(s_block_positions, true);

        //// Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
        //serializedObject.ApplyModifiedProperties();
    //}


}
