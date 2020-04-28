using System.Collections.Generic;

public interface IEnemyShipModelsProvider
{
    IList<EnemyShipModel> Models { get; }
}

public class EnemyShipModelsProvider : IEnemyShipModelsProvider
{
    private List<EnemyShipModel> _models = new List<EnemyShipModel>();

    public IList<EnemyShipModel> Models => _models;

    public void AddShip(EnemyShipModel model)
    {
        _models.Add(model);
    }
}
