using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SelectLevelScreenScrollItemInnerView : MonoBehaviour
{
    public event Action Clicked = delegate { };

    [SerializeField] private Image _bgIcon;
    [SerializeField] private Image _lockedIcon;
    [SerializeField] private Image _completedIcon;
    [SerializeField] private Text _levelNumText;
    [SerializeField] private Sprite _completedBg;
    [SerializeField] private Sprite _uncompletedBg;
    [SerializeField] private Sprite _currentUncompletedBg;
    [SerializeField] private Sprite _currentCompletedBg;
    [SerializeField] private Button _button;

    private bool _isCompleted;
    private bool _isCurrent;

    private void Awake()
    {
        _button.onClick.AddListener(() => Clicked());
    }

    public void SetLevelNum(int num)
    {
        _levelNumText.text = num.ToString();
    }

    public void SetLocked(bool isLocked)
    {
        _lockedIcon.gameObject.SetActive(isLocked);
    }

    public void SetCompleted(bool isCompleted)
    {
        _isCompleted = isCompleted;
        _completedIcon.gameObject.SetActive(isCompleted);

        UpdateBG();
    }

    public void SetCurrent(bool isCurrent)
    {
        _isCurrent = isCurrent;

        UpdateBG();
    }

    private void UpdateBG()
    {
        _bgIcon.sprite = _isCompleted
            ? (_isCurrent ? _currentCompletedBg : _completedBg)
            : (_isCurrent ? _currentUncompletedBg : _uncompletedBg);

        StopAllCoroutines();
        if (_isCurrent)
        {
            StartCoroutine(RotateBG());
        } else
        {
            _bgIcon.rectTransform.rotation = Quaternion.identity;
        }
    }

    private IEnumerator RotateBG()
    {
        while(true)
        {
            _bgIcon.rectTransform.eulerAngles -= _bgIcon.rectTransform.forward;

            yield return new WaitForEndOfFrame();
        }
    }
}
