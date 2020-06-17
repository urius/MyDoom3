using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelsConfigProvider))]
public class LevelsConfigUpdater : Editor
{
    private LevelsConfigProvider _target;

    public void OnEnable()
    {
        _target = (LevelsConfigProvider)serializedObject.targetObject;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (_target.LevelConfigs != null)
        {
            for (int i = 0; i < _target.LevelConfigs.Length; i++)
            {
                _target.LevelConfigs[i].Index = i;
            }
        }

        EditorUtility.SetDirty(_target);
    }
}
