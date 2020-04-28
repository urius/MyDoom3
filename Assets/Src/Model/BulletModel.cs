using UnityEngine;

public class BulletModel
{
    public readonly TeamId Team;
    public readonly Transform Transform;
    public readonly float Speed;
    public readonly int Damage;
    public readonly GameObject SparksPrefab;

    public BulletModel(Transform transform, WeaponConfig config, TeamId team)
    {
        Transform = transform;
        Team = team;
        Speed = config.BulletSpeed;
        Damage = config.Damage;
        SparksPrefab = config.SparksPrefab;
    }
}
