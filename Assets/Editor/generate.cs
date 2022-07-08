using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(TerrainGeneration))]
public class LevelScriptEditor : Editor
{
   
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        TerrainGeneration tg = (TerrainGeneration)target;
        
        if (GUILayout.Button("Build Object"))
        {
            tg.generate();
            tg.SplatMap();
            
        }
        //EditorGUILayout.HelpBox("This is a help box", MessageType.Info);

    }
}