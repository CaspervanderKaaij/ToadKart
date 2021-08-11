using System.Collections;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (AllCheckpoints))]
public class AllCheckpointsEditor : Editor {
    public override void OnInspectorGUI () {
        DrawDefaultInspector ();

        AllCheckpoints allCheckpoints = (AllCheckpoints) target;

        if (GUILayout.Button ("Setup all checkpoints")) {
            allCheckpoints.Setup();
        }

    }
}