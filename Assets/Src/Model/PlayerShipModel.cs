using UnityEngine;

public class PlayerShipModel : ShipModel
{
    public PlayerShipModel(Vector3[] weaponSlotlocalPositions, Collider[] colliders, ShipData shipStaticData)
        : base(TeamId.Player, weaponSlotlocalPositions, colliders, shipStaticData)
    {
    }
}
