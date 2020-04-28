using System.Collections.Generic;

public interface IBulletsModelProvider
{
    IList<BulletModel> Models { get; }

    void Add(BulletModel model);
}

public class BulletsModelProvider : IBulletsModelProvider
{
    private readonly List<BulletModel> _models = new List<BulletModel>();

    public IList<BulletModel> Models => _models;

    public void Add(BulletModel model)
    {
        _models.Add(model);
    }
}
