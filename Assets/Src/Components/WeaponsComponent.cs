using System.Linq;
using UnityEngine;

public class WeaponsComponent : MonoBehaviour
{
    [SerializeField]
    private Transform[] _weaponSlots;

    public Vector3[] WeaponSlotPositions => _weaponSlots.Select(s => s.position).ToArray();
    public Transform[] WeaponTransforms => _weaponSlots;
}
