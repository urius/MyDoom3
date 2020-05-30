using System;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

public enum EquipmentSlotType
{
    Default,
    Weapon,
    Engine,
    Shield,
}

public class EquipmentSlotView : MonoBehaviour
{
    [SerializeField]
    private Button _button;
    [SerializeField]
    private Image _equipmentIconImage;
    [SerializeField]
    private Image _bgImage;
    [SerializeField]
    private Collider2D _itemCollider;


    [SerializeField]
    private ColorBySlotType[] _colours;

    [SerializeField]
    private Color _defaultTypeColor;
    [SerializeField]
    private Color _weaponTypeColor;
    [SerializeField]
    private Color _engineTypeColor;
    [SerializeField]
    private Color _shieldTypeColor;

    public Action OnMouseDown = delegate { };
    public Collider2D ItemCollider => _itemCollider;
    public bool IconVisibility
    {
        get => _equipmentIconImage.enabled;
        set { _equipmentIconImage.enabled = value; }
    }

    public void SetSlotType(EquipmentSlotType type)
    {
        var colorEl = Array.Find(_colours, c => c.EquipmentSlotType == type);
        _bgImage.color = colorEl.Color;
    }

    public void SetSpriteIcon(Sprite iconSprite)
    {
        _equipmentIconImage.sprite = iconSprite;
        IconVisibility = (iconSprite != null);
    }

    protected void OnPointerDown()
    {
        OnMouseDown();
    }
}

[Serializable]
class ColorBySlotType
{
    public EquipmentSlotType EquipmentSlotType;
    public Color Color;
}
