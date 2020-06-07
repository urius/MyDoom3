using System;
using UnityEngine;

[Serializable]
public class ShipDataMin
{
    [SerializeField]
    private ShipType _shipType;
    [SerializeField]
    private WeaponId[] _weaponIds;
    [SerializeField]
    private ShieldId _shieldId;
    [SerializeField]
    private EngineId _engineId;

    public ShipDataMin(ShipType shipType, WeaponId[] weaponIds, ShieldId shieldId, EngineId engineId)
    {
        _shipType = shipType;
        _weaponIds = weaponIds;
        _shieldId = shieldId;
        _engineId = engineId;
    }

    public WeaponId[] WeaponIds => _weaponIds;
    public ShipType ShipType => _shipType;
    public ShieldId ShieldId => _shieldId;
    public EngineId EngineId => _engineId;
}
