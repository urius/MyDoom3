using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "EnginesConfig", menuName = "Configs/EnginesConfig")]
public class EnginesConfigProvider : ScriptableObject
{
    [SerializeField]
    private EngineConfig[] _configs;

    public EngineConfig GetConfigByEngineId(EngineId engine)
    {
        return _configs.First(c => c.EngineId == engine);
    }

    public EngineConfig[] Configs => _configs;
}

public enum EngineId
{
    Undefined,
    RocketEngine_x2,
    NuclearEngine,
    NuclearEngine_x2,
    QuantumEngine,
    RocketEngine,
}

[Serializable]
public class EngineConfig : EquipmentConfigBase
{
    public EngineId EngineId;
    public float Speed;
    public float Mobility;
    public float RotationSpeed;

    public override EquipmentType EquipmentType => EquipmentType.Engine;

    public override EquipmentMin ToEquipmentMin()
    {
        return new EquipmentMin(EquipmentType, (int)EngineId);
    }
}
