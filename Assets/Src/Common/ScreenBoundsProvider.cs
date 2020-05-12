using System;
using UnityEngine;

public class ScreenBoundsProvider
{
    private Rect _worldBoundsRect;

    public Rect Bounds => _worldBoundsRect.width == 0 ? CreateRect() : _worldBoundsRect;

    public void MoveBoundsCenterZ(float value)
    {
        _worldBoundsRect.center = new Vector2(_worldBoundsRect.center.x, _worldBoundsRect.center.y + value);
    }

    public void UpdateBounds()
    {
        CreateRect();
    }

    private Rect CreateRect()
    {
        var camera = Camera.main;
        var topRightPos = ScreenToWorldPlanePoint(camera, Screen.width, Screen.height);
        var bottomLeftPos = ScreenToWorldPlanePoint(camera, 0, 0);
        _worldBoundsRect.Set(bottomLeftPos.x, bottomLeftPos.z, topRightPos.x - bottomLeftPos.x, topRightPos.z - bottomLeftPos.z);

        return _worldBoundsRect;
    }

    private static Vector3 ScreenToWorldPlanePoint(Camera camera, int screenX, int screenY)
    {
        var ray = camera.ScreenPointToRay(new Vector3(screenX, screenY, 0));
        var angleBetweenUp = Mathf.Deg2Rad * Vector3.Angle(-Vector3.up, ray.direction);
        var vectorLength = camera.transform.position.y / (float)Math.Cos(angleBetweenUp);

        return ray.GetPoint(vectorLength);
    }
}
