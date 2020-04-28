using System.Linq;
using UnityEngine;

public class CollidersComponent : MonoBehaviour
{
    [SerializeField]
    private Collider[] _colliders;

    public Collider[] Colliders => _colliders;
    public Bounds[] BoundsArray => _colliders.Select(c => c.bounds).ToArray();
}
