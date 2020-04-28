using System;
using System.Linq;
using UnityEngine;

public class ShipModel
{
    public event Action PositionChanged = delegate { };
    public event Action RotationChanged = delegate { };
    public event Action Disposed = delegate { };
    public event Action<int, WeaponConfig, TeamId> Fired = delegate { };

    public readonly TeamId Team;
    public readonly Vector3[] WeaponSlotsLocal;
    public readonly Collider[] Colliders;

    public int HP;

    protected readonly ShipData _shipStaticData;

    private readonly WeaponConfig[] _weaponConfigs;
    private readonly int[] _cooldownStartValues;
    private readonly int[] _cooldowns;

    private Vector3 _position;
    private Quaternion _rotation;

    public ShipModel(TeamId team, Vector3[] weaponLocalPositions, Collider[] colliders, ShipData shipStaticData)
    {
        Team = team;
        WeaponSlotsLocal = weaponLocalPositions;
        Colliders = colliders;

        _shipStaticData = shipStaticData;

        HP = shipStaticData.ShipConfig.HP;

        _weaponConfigs = shipStaticData.WeaponsConfig;
        _cooldownStartValues = shipStaticData.WeaponsConfig.Select(c => c.CooldownFrames).ToArray();
        _cooldowns = (int[])_cooldownStartValues.Clone();
    }

    public float Speed => _shipStaticData.ShipConfig.Speed;
    public float Mobility => _shipStaticData.ShipConfig.Mobility;
    public float RotationSpeed => _shipStaticData.ShipConfig.RotationSpeed;

    public Vector3 Forward => _rotation * Vector3.forward;
    public bool IsDisposed { get; private set; }

    private bool CanFire => HP > 0 && _cooldowns.Any(c => c <= 0);

    public Vector3 Position
    {
        get => _position;
        set
        {
            _position = value;
            PositionChanged();
        }
    }

    public Quaternion Rotation
    {
        get => _rotation;
        set
        {
            _rotation = value;
            RotationChanged();
        }
    }

    public void UpdateCooldowns()
    {
        for (var i = 0; i < _cooldowns.Length; i++)
        {
            if (_cooldowns[i] > 0)
            {
                _cooldowns[i]--;
            }
        }
    }

    public void FireAllWeapons()
    {
        if (CanFire)
        {
            for (var i = 0; i < _cooldowns.Length; i++)
            {
                if (_cooldowns[i] <= 0)
                {
                    Fired(i, _weaponConfigs[i], Team);
                    _cooldowns[i] = _cooldownStartValues[i];
                }
            }
        }
    }

    public void Dispose()
    {
        IsDisposed = true;
        Disposed();
    }
}
