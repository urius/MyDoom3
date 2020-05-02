using System;
using UnityEngine;

public class EventsAggregator
{
    public Action<Transform, WeaponConfig, TeamId> BulletViewCreated = delegate { };
    public Action Ticked = delegate { };
    public Action<ShipModel, BulletModel> CollisionHappened = delegate { };
}
