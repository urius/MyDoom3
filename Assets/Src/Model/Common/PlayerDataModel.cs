using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

public class PlayerDataModel
{
    private const string SaveFileName = "pd.sv";

    public event Action<EquipmentData> InventoryEquipmentRemoved = delegate { };
    public event Action<int, EquipmentData> InventoryEquipmentSet = delegate { };
    public event Action RequestSave = delegate { };

    private readonly TaskCompletionSource<bool> _dataLoadedTsc = new TaskCompletionSource<bool>();

    public int Money;
    public int Exp;
    public ShipData ShipData;

    private List<EquipmentData> _inventoryEqipments = new List<EquipmentData>();
    private readonly ModelsFactory _modelsFactory;
    private readonly DefaultPlayerDataProvider _defaultPlayerDataProvider;

    [Inject]
    public PlayerDataModel(
        ModelsFactory modelsFactory,
        DefaultPlayerDataProvider defaultPlayerDataProvider)
    {
        _modelsFactory = modelsFactory;
       _defaultPlayerDataProvider = defaultPlayerDataProvider;
    }

    public ShipConfig ShipConfig => ShipData.ShipConfig;
    public IReadOnlyList<EquipmentData> InventoryEqipments => _inventoryEqipments;
    public Task DataLoadedTask => _dataLoadedTsc.Task;

    public void AddInventoryEquipment(EquipmentData equipment)
    {
        _inventoryEqipments.Add(equipment);

        InventoryEquipmentSet(_inventoryEqipments.Count - 1, equipment);
    }

    public void RemoveInventoryEquipment(EquipmentData equipment)
    {
        _inventoryEqipments.Remove(equipment);

        InventoryEquipmentRemoved(equipment);
    }

    public void SwapInventoryEquipment(EquipmentData equipmentInInventoryToRemove, EquipmentData newEquipment)
    {
        var index = _inventoryEqipments.IndexOf(equipmentInInventoryToRemove);
        _inventoryEqipments[index] = newEquipment;

        InventoryEquipmentSet(index, newEquipment);
    }

    public bool HaveEquipmentInInventory(EquipmentData equipment)
    {
        return _inventoryEqipments.IndexOf(equipment) > -1;
    }

    public void Save()
    {
        SaveLoadHelper.SaveSerialized(toPlayerDataMin(), SaveFileName);
    }

    public void Load()
    {
        if (!SaveLoadHelper.TryLoadSerialized<PlayerDataMin>(SaveFileName, out var data))
        {
            data = _defaultPlayerDataProvider.PlayerData;
        }

        initFromMinData(data);

        _dataLoadedTsc.TrySetResult(true);
    }

    private PlayerDataMin toPlayerDataMin()
    {
        return new PlayerDataMin(
            Money,
            Exp,
            ShipData.ToShipDataMin(),
            _inventoryEqipments.Select(e => e.ToEquipmentMin()).ToArray());
    }

    private void initFromMinData(PlayerDataMin dataMin)
    {
        Money = dataMin.Money;
        Exp = dataMin.Exp;
        ShipData = _modelsFactory.CreateShipData(dataMin.ShipData);
        _inventoryEqipments = dataMin.InventoryEqipmentsMin.Select(_modelsFactory.CreateEquipment).ToList();
    }
}

[Serializable]
public class PlayerDataMin
{
    public PlayerDataMin(int money, int exp, ShipDataMin shipData, EquipmentMin[] inventoryEqipmentsMin)
    {
        Money = money;
        Exp = exp;
        ShipData = shipData;
        InventoryEqipmentsMin = inventoryEqipmentsMin;
    }

    public int Money;
    public int Exp;
    public ShipDataMin ShipData;
    public EquipmentMin[] InventoryEqipmentsMin;
}
