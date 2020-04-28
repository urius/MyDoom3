using System;

public interface IPlayerShipModelProvider
{
    event Action<ShipModel> ShipModelSet;

    ShipModel ShipModel { get; }
}

public class PlayerShipModelHolder : IPlayerShipModelProvider
{
    public event Action<ShipModel> ShipModelSet = delegate { };

    private ShipModel _shipModel;
    public ShipModel ShipModel
    {
        get => _shipModel;
        set
        {
            _shipModel = value;
            ShipModelSet(value);
        }
    }
}