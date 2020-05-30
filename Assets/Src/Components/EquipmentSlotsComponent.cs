using UnityEngine;

public class EquipmentSlotsComponent : MonoBehaviour
{
    [SerializeField]
    private Transform[] _shieldSlots;
    [SerializeField]
    private Transform[] _engineSlots;

    public Transform[] ShieldSlots => _shieldSlots;
    public Transform[] EngineSlots => _engineSlots;
}
