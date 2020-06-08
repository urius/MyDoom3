using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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

    private RectTransform _rectTransform;

    private float _targetScale = 1;
    private Coroutine _scaleCoroutine;

    public Action OnMouseDown = delegate { };
    public Collider2D ItemCollider => _itemCollider;
    public bool IconVisibility
    {
        get => _equipmentIconImage.enabled;
        set { _equipmentIconImage.enabled = value; }
    }

    public void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
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

    public void AnimateScaleTo(float targetScaleValue = 1)
    {
        if (_scaleCoroutine != null)
        {
            StopCoroutine(_scaleCoroutine);
            _scaleCoroutine = null;
        }

        _targetScale = targetScaleValue;
        StartCoroutine(ScaleCoroutine());
    }

    private IEnumerator ScaleCoroutine()
    {
        while (Math.Abs(_targetScale - _rectTransform.localScale.x) > 0.01)
        {
            _rectTransform.localScale = Vector3.Lerp(_rectTransform.localScale, new Vector3(_targetScale, _targetScale, 1), 0.25f);
            yield return new WaitForSeconds(Time.deltaTime);
        }
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
