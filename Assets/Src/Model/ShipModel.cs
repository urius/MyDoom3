using System;
using System.Collections.Generic;
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

    public const int DestroyDurationThresholdFrames = 500;

    public readonly TeamId Team;

    public int DestroyDurationFrames;

    protected readonly ShipData _shipStaticData;

    private readonly IReadOnlyList<(WeaponConfig config, int slotIndex)> _installedWeaponConfigs;
    private readonly int[] _cooldownStartValues;
    private readonly int[] _cooldowns;

    private Vector3 _position;
    private Quaternion _rotation;
    private int _hp;


    public ShipModel(TeamId team, ShipData shipStaticData)
    {
        Team = team;

        _shipStaticData = shipStaticData;

        HP = shipStaticData.ShipConfig.HP;

        _installedWeaponConfigs = shipStaticData.WeaponsConfig.Select((c, i) => (c, i)).Where(pair => pair.c != null).ToArray();
        _cooldownStartValues = _installedWeaponConfigs.Select(p => p.config.CooldownFrames).ToArray();
        _cooldowns = (int[])_cooldownStartValues.Clone();
    }

    private bool CanFire => HP > 0 && _cooldowns.Any(c => c <= 0);

    public float Speed => _shipStaticData.ShipConfig.Speed;
    public float Mobility => _shipStaticData.ShipConfig.Mobility;
    public float RotationSpeed => _shipStaticData.ShipConfig.RotationSpeed;
    public GameObject ShipPrefab => _shipStaticData.ShipConfig.ShipPrefab;
    public GameObject ExplosionPrefab => _shipStaticData.ShipConfig.ExplosionPrefab;
    public GameObject DestroyedShipPrefab => _shipStaticData.ShipConfig.DestroyedShipPrefab;

    public Vector3 Forward => _rotation * Vector3.forward;
    public bool IsDestroyState { get; private set; }
    public bool IsDisposed { get; private set; }

    public int HP
    {
        get => _hp;
        set
        {
            var hpBefore = _hp;
            _hp = value;
            HpChanged(_hp - hpBefore);
            if (_hp <= 0 && IsDestroyState == false)
            {
                IsDestroyState = true;
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
                    Fired(_installedWeaponConfigs[i].slotIndex, _installedWeaponConfigs[i].config, Team);
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
