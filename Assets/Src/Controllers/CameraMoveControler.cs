using System;
using UnityEngine;
using Zenject;

public class CameraMoveControler : IInitializable, ITickable
{
    private const int UpdateScreenBoundsIntervalFrames = 100;

    private Camera _camera;
    private ScreenBoundsProvider _screenBoundsProvider;
    private IPlayerShipModelProvider _playerShipModelProvider;
    private int _framesToUpdateBoundsCounter;

    [Inject]
    public void Construct(
        ScreenBoundsProvider screenBoundsProvider,
        IPlayerShipModelProvider playerShipModelProvider)
    {
        _screenBoundsProvider = screenBoundsProvider;
        _playerShipModelProvider = playerShipModelProvider;
    }

    public void Initialize()
    {
        _camera = Camera.main;
    }

    public void Tick()
    {
        if (_playerShipModelProvider.ShipModel != null)
        {
            _camera.transform.position += new Vector3(0, 0, _playerShipModelProvider.ShipModel.Speed);
            _screenBoundsProvider.MoveBoundsCenterZ(_playerShipModelProvider.ShipModel.Speed);
        }

        _framesToUpdateBoundsCounter++;
        if (_framesToUpdateBoundsCounter > UpdateScreenBoundsIntervalFrames)
        {
            _framesToUpdateBoundsCounter = 0;
            _screenBoundsProvider.UpdateBounds();
        }

#if UNITY_EDITOR
        DrawSceenBounds();
#endif
    }

    private void DrawSceenBounds()
    {
        var bounds = _screenBoundsProvider.Bounds;
        Debug.DrawLine(new Vector3(bounds.min.x, 0, bounds.min.y), new Vector3(bounds.min.x, 0, bounds.max.y));
        Debug.DrawLine(new Vector3(bounds.min.x, 0, bounds.max.y), new Vector3(bounds.max.x, 0, bounds.max.y));
        Debug.DrawLine(new Vector3(bounds.max.x, 0, bounds.max.y), new Vector3(bounds.max.x, 0, bounds.min.y));
        Debug.DrawLine(new Vector3(bounds.max.x, 0, bounds.min.y), new Vector3(bounds.min.x, 0, bounds.min.y));
    }
}
