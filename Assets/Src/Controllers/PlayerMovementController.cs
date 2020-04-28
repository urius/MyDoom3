using UnityEngine;
using Zenject;

public class PlayerMovementController : ITickable
{
    private const float LookAtDistanceScreens = 0.5f;
    private const int _targetYOffset = 10;

    private IPlayerShipModelProvider _playerShipModelProvider;
    private Rect _worldBoundsRect;

    [Inject]
    public void Construct(
        IPlayerShipModelProvider playerShipModelProvider,
        ScreenBoundsProvider screenBoundsProvider)
    {
        _playerShipModelProvider = playerShipModelProvider;
        _worldBoundsRect = screenBoundsProvider.Bounds;
    }

    public void Tick()
    {
        if (_playerShipModelProvider.ShipModel == null)
        {
            return;
        }

        _playerShipModelProvider.ShipModel.UpdateCooldowns();

        var shipModel = _playerShipModelProvider.ShipModel;
        var playerPosition = shipModel.Position;
        var mobility = _playerShipModelProvider.ShipModel.Mobility;
        var mousePosNorm = new Vector3(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);

        var targetX = _worldBoundsRect.x + mousePosNorm.x * _worldBoundsRect.width;
        var targetZ = _worldBoundsRect.y + mousePosNorm.y * _worldBoundsRect.height + _targetYOffset;
        var newX = playerPosition.x + (targetX - playerPosition.x) * mobility;
        var newZ = playerPosition.z + (targetZ - playerPosition.z) * mobility;
        shipModel.Position = new Vector3(newX, 0, newZ);

        var lookAtZ = targetZ - newZ + _worldBoundsRect.height * LookAtDistanceScreens;
        shipModel.Rotation = Quaternion.LookRotation(new Vector3(targetX - newX, 0, lookAtZ), Vector3.up);

        if (Input.GetMouseButton(0))
        {
            _playerShipModelProvider.ShipModel.FireAllWeapons();
        }
    }
}
