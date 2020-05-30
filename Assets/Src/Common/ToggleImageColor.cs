using UnityEngine;
using UnityEngine.UI;

public class ToggleImageColor : MonoBehaviour
{
    [SerializeField]
    private Toggle _toggle;
    [SerializeField]
    private Image _image;
    [SerializeField]
    private Color _selectedColor;

    private Color _imageDefaultColor;

    private void Awake()
    {
        _imageDefaultColor = _image.color;
    }

    private void OnEnable()
    {
        _toggle.onValueChanged.AddListener(OnValueChanged);
    }

    private void Start()
    {
        UpdateColor(_toggle.isOn);
    }

    private void OnValueChanged(bool isSelected)
    {
        UpdateColor(isSelected);
    }

    private void UpdateColor(bool isSelected)
    {
        _image.color = isSelected ? _selectedColor : _imageDefaultColor;
    }
}
