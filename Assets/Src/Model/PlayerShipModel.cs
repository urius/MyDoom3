using UnityEngine;

public class PlayerShipModel : ShipModel
{
    public PlayerShipModel(ShipData shipStaticData)
        : base(TeamId.Player, shipStaticData)
    {
    }
}
