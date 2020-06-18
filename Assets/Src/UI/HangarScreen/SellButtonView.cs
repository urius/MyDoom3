using System;
using System.Threading.Tasks;
using DigitalRuby.Tween;
using UnityEngine;
using UnityEngine.UI;

public class SellButtonView : MonoBehaviour
{
    [SerializeField] private Text _priceText;
    [SerializeField] private Collider2D _collider;

    private const string ScaleTweenId = "ScaleTween";
    private readonly Vector2 FadeOutScale = new Vector2(0.1f, 0.1f);

    private RectTransform _rectTransform;
    private Vector2 _defaultScale;

    public void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _defaultScale = _rectTransform.localScale;
    }

    public void SetPriceText(string text)
    {
        _priceText.text = text;
    }

    public void Show(bool animated = false)
    {
        gameObject.SetActive(true);

        TweenFactory.RemoveTweenKey(ScaleTweenId, TweenStopBehavior.DoNotModify);
        if (animated)
        {
            gameObject.Tween(ScaleTweenId, _rectTransform.localScale, _defaultScale, 0.4f, TweenScaleFunctions.QuinticEaseOut, OnTweenScale);
        }
        else
        {
            _rectTransform.localScale = _defaultScale;
        }
    }

    public Task HideAsync(bool animated = false)
    {
        TweenFactory.RemoveTweenKey(ScaleTweenId, TweenStopBehavior.DoNotModify);

        var tsc = new TaskCompletionSource<bool>();
        void OnTweenEnded(ITween<Vector2> _ = null)
        {
            gameObject.SetActive(false);
            tsc.TrySetResult(true);
        }

        if (animated)
        {
            gameObject.Tween(ScaleTweenId, _rectTransform.localScale, FadeOutScale, 0.3f, TweenScaleFunctions.QuadraticEaseIn, OnTweenScale, OnTweenEnded);
        }
        else
        {
            _rectTransform.localScale = FadeOutScale;
            OnTweenEnded();
        }

        return tsc.Task;
    }

    public bool HitTestPoint(Vector3 point)
    {
        return _collider.bounds.ClosestPoint(point) == point;
    }

    private void OnDisable()
    {
        TweenFactory.RemoveTweenKey(ScaleTweenId, TweenStopBehavior.DoNotModify);
    }

    private void OnTweenScale(ITween<Vector2> tween)
    {
        _rectTransform.localScale = tween.CurrentValue;
    }
}
