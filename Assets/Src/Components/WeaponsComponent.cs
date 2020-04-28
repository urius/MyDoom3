using System.Linq;
using UnityEngine;

public class WeaponsComponent : MonoBehaviour
{
    [SerializeField]
    private Transform[] _weaponSlots;

    public Transform[] WeaponTransforms => _weaponSlots;
}
