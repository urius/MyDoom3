using System;
using UnityEngine;
using UnityEngine.UI;

public class BuyEquipmentScrollItemView : MonoBehaviour
{
    public event Action Clicked = delegate { };

    [SerializeField]
    private Button _button;
    [SerializeField]
    private Image _image;
    [SerializeField]
    private Text _captionTxt;
    [SerializeField]
    private Text _infoTxt;
    [SerializeField]
    private Text _costTxt;

    private void OnEnable()
    {
        _button.onClick.AddListener(() => Clicked());
    }

    private void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
    }

    public void SetImage(Sprite sprite)
    {
        _image.sprite = sprite;
    }

    public void SetCaptionText(string text)
    {
        _captionTxt.text = text;
    }

    public void SetInfoText(string text)
    {
        _infoTxt.text = text;
    }

    public void SetCostText(string text)
    {
        _costTxt.text = text;
    }
}
