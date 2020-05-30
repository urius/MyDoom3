using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ButtonView : MonoBehaviour
{
    [SerializeField]
    private Button _button;

    public event Action Clicked = delegate { };

    public void SetEnabled(bool isEnabled)
    {
        _button.interactable = isEnabled;
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(() => Clicked());
    }

    private void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
    }
}
