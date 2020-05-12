using UnityEngine;
using Zenject;

public class PlayerMovementController : ITickable
{
    private const float LookAtDistanceScreens = 0.2f;
    private const int _targetYOffset = 20;

    private IPlayerShipModelProvider _playerShipModelProvider;
    private ScreenBoundsProvider _screenBoundsProvider;

    [Inject]
    public void Construct(
        IPlayerShipModelProvider playerShipModelProvider,
        ScreenBoundsProvider screenBoundsProvider)
    {
        _playerShipModelProvider = playerShipModelProvider;
        _screenBoundsProvider = screenBoundsProvider;
    }

    public void Tick()
    {
        var shipModel = _playerShipModelProvider.ShipModel;
        if (shipModel == null || shipModel.IsDestroyState)
        {
            return;
        }

        var worldBoundsRect = _screenBoundsProvider.Bounds;

        var playerPosition = shipModel.Position;
        var mobility = _playerShipModelProvider.ShipModel.Mobility;
        var mousePosNorm = new Vector3(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);

        var targetX = worldBoundsRect.min.x + mousePosNorm.x * worldBoundsRect.width;
        var targetZ = worldBoundsRect.min.y + mousePosNorm.y * worldBoundsRect.height + _targetYOffset;
        var newX = playerPosition.x + (targetX - playerPosition.x) * mobility;
        var newZ = playerPosition.z + (targetZ - playerPosition.z) * mobility;
        shipModel.Position = new Vector3(newX, 0, newZ);

        var lookAtZ = targetZ - newZ + worldBoundsRect.height * LookAtDistanceScreens;
        shipModel.Rotation = Quaternion.LookRotation(new Vector3(targetX - newX, 0, lookAtZ), Vector3.up);

        shipModel.UpdateCooldowns();
        if (Input.GetMouseButton(0))
        {
            shipModel.FireAllWeapons();
        }
    }
}
