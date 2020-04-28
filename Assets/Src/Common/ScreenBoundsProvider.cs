using System;
using UnityEngine;

public class ScreenBoundsProvider
{
    private Rect _worldBoundsRect;

    public Rect Bounds => _worldBoundsRect.width == 0 ? CreateRect() : _worldBoundsRect;

    public void MoveBoundsCenterZ(float value)
    {
        _worldBoundsRect.center = new Vector2(0, value);
    }

    private Rect CreateRect()
    {
        var camera = Camera.main;
        var topRightPos = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Math.Abs(camera.transform.position.y)));
        var bottomLeftPos = camera.ScreenToWorldPoint(new Vector3(0, 0, Math.Abs(camera.transform.position.y)));
        _worldBoundsRect.Set(bottomLeftPos.x, bottomLeftPos.z, topRightPos.x - bottomLeftPos.x, topRightPos.z - bottomLeftPos.z);
        return _worldBoundsRect;
    }
}
