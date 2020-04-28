using System;
using UnityEngine;

public class EventsAggregator
{
    public Action<Transform, WeaponConfig, TeamId> BulletViewCreated = delegate { };
}
