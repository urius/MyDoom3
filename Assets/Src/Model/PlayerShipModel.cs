using UnityEngine;

public class PlayerShipModel : ShipModel
{
    public PlayerShipModel(Collider[] colliders, ShipData shipStaticData)
        : base(TeamId.Player, colliders, shipStaticData)
    {
    }
}
