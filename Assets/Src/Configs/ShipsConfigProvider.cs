using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ShipsConfig", menuName = "Configs/ShipsConfig")]
public class ShipsConfigProvider : ScriptableObject
{
    [SerializeField]
    private ShipConfig[] _configs;

    public ShipConfig GetConfigByShipType(ShipType shipType)
    {
        return _configs.First(c => c.ShipType == shipType);
    }
}

public enum ShipType
{
    StarSparrow,
}

[Serializable]
public class ShipConfig
{
    public ShipType ShipType;
    public GameObject ShipPrefab;
    public GameObject ExplosionPrefab;
    public GameObject DestroyedShipPrefab;
    public float Speed;
    public float Mobility;
    public float RotationSpeed;
    public int HP;
}

