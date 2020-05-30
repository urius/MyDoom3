using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class StaticLocalizator : MonoBehaviour
{
    [SerializeField]
    private LocalizationUnit[] _localizations;

    private LocalizationProvider _localizationProvider;

    [Inject]
    private void Construct(LocalizationProvider localizationProvider)
    {
        _localizationProvider = localizationProvider;
    }

    public void UpdateTexts()
    {
        foreach (var unit in _localizations)
        {
            unit.Text.text = _localizationProvider.GetLocalization(unit.LocalizationsGroup, unit.LocalizationKey);
        }
    }

    private void Awake()
    {
        UpdateTexts();
    }
}

[Serializable]
class LocalizationUnit
{
    public Text Text;
    public LocalizationGroupId LocalizationsGroup;
    public string LocalizationKey;
}
