using System;
using System.Linq;
using UnityEngine;

public class ShipModel
{
    public event Action PositionChanged = delegate { };
    public event Action RotationChanged = delegate { };
    public event Action Disposed = delegate { };
    public event Action<int, WeaponConfig, TeamId> Fired = delegate { };
    public event Action<int> HpChanged = delegate { };
    public event Action DestroyStarted = delegate { };

    public readonly TeamId Team;

    protected readonly ShipData _shipStaticData;

    private readonly WeaponConfig[] _weaponConfigs;
    private readonly int[] _cooldownStartValues;
    private readonly int[] _cooldowns;

    private Vector3 _position;
    private Quaternion _rotation;
    private int _hp;

    public ShipModel(TeamId team, Collider[] colliders, ShipData shipStaticData)
    {
        Team = team;
        Colliders = colliders;

        _shipStaticData = shipStaticData;

        HP = shipStaticData.ShipConfig.HP;

        _weaponConfigs = shipStaticData.WeaponsConfig;
        _cooldownStartValues = shipStaticData.WeaponsConfig.Select(c => c.CooldownFrames).ToArray();
        _cooldowns = (int[])_cooldownStartValues.Clone();
    }

    private bool CanFire => HP > 0 && _cooldowns.Any(c => c <= 0);

    public float Speed => _shipStaticData.ShipConfig.Speed;
    public float Mobility => _shipStaticData.ShipConfig.Mobility;
    public float RotationSpeed => _shipStaticData.ShipConfig.RotationSpeed;
    public GameObject ExplosionPrefab => _shipStaticData.ShipConfig.ExplosionPrefab;
    public GameObject DestroyedShipPrefab => _shipStaticData.ShipConfig.DestroyedShipPrefab;

    public Vector3 Forward => _rotation * Vector3.forward;
    public bool IsDestroying { get; private set; }
    public bool IsDisposed { get; private set; }
    public Collider[] Colliders { get; private set; }

    public int HP
    {
        get => _hp;
        set
        {
            var hpBefore = _hp;
            _hp = value;
            HpChanged(_hp - hpBefore);
            if (_hp <= 0 && IsDestroying == false)
            {
                Colliders = Array.Empty<Collider>();
                IsDestroying = true;
                DestroyStarted();
            }
        }
    }

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
