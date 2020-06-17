using UnityEditor;

[CustomEditor(typeof(DefaultPlayerDataProvider))]
public class DefaultPlayerDataLevelsIndexUpdater : Editor
{
    private DefaultPlayerDataProvider _target;

    private void OnEnable()
    {
        _target = (DefaultPlayerDataProvider)serializedObject.targetObject;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (_target.PlayerData.LevelsMin != null)
        {
            for (int i = 0; i < _target.PlayerData.LevelsMin.Length; i++)
            {
                _target.PlayerData.LevelsMin[i].Index = i;
            }
        }
    }
}
