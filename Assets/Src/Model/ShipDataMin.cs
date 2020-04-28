using System;
using UnityEngine;

[Serializable]
public class ShipDataMin
{
    [SerializeField]
    private ShipType _shipType;
    [SerializeField]
    private WeaponId[] _weaponIds;

    public ShipDataMin(ShipType shipType, WeaponId[] weaponIds)
    {
        _shipType = shipType;
        _weaponIds = weaponIds;
    }

    public WeaponId[] WeaponIds => _weaponIds;
    public ShipType ShipType => _shipType;
}
