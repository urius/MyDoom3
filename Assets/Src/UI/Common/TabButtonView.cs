using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TabButtonView : MonoBehaviour
{
    [SerializeField]
    private Toggle _toggle;

    public event Action Selected = delegate { };

    public void SetSelected()
    {
        _toggle.isOn = true;
    }

    private void OnEnable()
    {
        _toggle.onValueChanged.AddListener(v =>
        {
            if (v == true)
            {
                Selected();
            }
        });
    }

    private void OnDisable()
    {
        _toggle.onValueChanged.RemoveAllListeners();
    }
}
