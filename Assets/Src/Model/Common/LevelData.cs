using System;
using UnityEngine;

public class LevelData
{
    public bool IsLocked;
    public bool IsCompleted;
    private LevelConfig _config;

    public LevelData(LevelDataMin dataMin, LevelConfig config)
    {
        IsCompleted = dataMin.IsCompleted;
        IsLocked = dataMin.IsLocked;

        _config = config;
    }

    public int LevelIndex => _config.Index;

    public LevelDataMin ToMinData()
    {
        return new LevelDataMin(_config.Index, IsLocked, IsCompleted);
    }
}

[Serializable]
public class LevelDataMin
{
    [HideInInspector]
    public int Index;
    public bool IsLocked;
    public bool IsCompleted;

    public LevelDataMin(int index, bool isLocked, bool isCompleted)
    {
        Index = index;
        IsLocked = isLocked;
        IsCompleted = isCompleted;
    }
}
