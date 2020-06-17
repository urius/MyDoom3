using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/LevelsConfig", fileName = "LevelsConfigProvider")]
public class LevelsConfigProvider : ScriptableObject
{
    [SerializeField]
    private LevelConfig[] _levelConfigs;

    public LevelConfig[] LevelConfigs => _levelConfigs;
}

[Serializable]
public class LevelConfig
{
    [HideInInspector]
    public int Index;
    public int WinPrice;
}

