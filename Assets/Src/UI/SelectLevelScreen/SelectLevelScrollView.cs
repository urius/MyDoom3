using System;
using System.Linq;
using DigitalRuby.Tween;
using UnityEngine;
using UnityEngine.UI;

public class SelectLevelScrollView : MonoBehaviour
{
    private const string ScrollTweenId = "scrollTween";

    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _rightButton;
    [SerializeField] private GameObject _scrollItemContainerPrefab;
    [SerializeField] private int _scrollItemContainerCapacity;
    [SerializeField] private GameObject _scrollItemInnerPrefab;
    [SerializeField] private RectTransform _content;

    private Transform[] _containers;
    private SelectLevelScreenScrollItemInnerView[] _itemViews;
    private float _itemContainerWidth;

    public void CreateItems(int count)
    {
        _itemViews = new SelectLevelScreenScrollItemInnerView[count];

        var containersNum = count / _scrollItemContainerCapacity + 1;

        Transform CreateContainer(int index)
        {
            var containerIndex = index / _scrollItemContainerCapacity;
            return Instantiate(_scrollItemContainerPrefab, _content).transform;
        }

        _containers = Enumerable.Range(0, containersNum).Select(CreateContainer).ToArray();

        SelectLevelScreenScrollItemInnerView CreateItem(int index)
        {
            var containerIndex = index / _scrollItemContainerCapacity;
            var go = Instantiate(_scrollItemInnerPrefab, _containers[containerIndex]);
            return go.GetComponent<SelectLevelScreenScrollItemInnerView>();
        }

        _itemViews = Enumerable.Range(0, count).Select(CreateItem).ToArray();
    }

    public void ShowOnIndex(int index)
    {
        var containerIndex = index / _scrollItemContainerCapacity;
        var containerPosition = ((RectTransform)_containers[containerIndex].transform).anchoredPosition;
        _content.anchoredPosition = new Vector2(-containerPosition.x, _content.anchoredPosition.y);
    }

    public SelectLevelScreenScrollItemInnerView GetItem(int index)
    {
        return _itemViews[index];
    }



    private void Awake()
    {
        _itemContainerWidth = ((RectTransform)_scrollItemContainerPrefab.transform).rect.width;

        _rightButton.onClick.AddListener(OnRightClick);
        _leftButton.onClick.AddListener(OnLeftClick);
    }

    private void OnDisable()
    {
        TweenFactory.RemoveTweenKey(ScrollTweenId, TweenStopBehavior.DoNotModify);
    }

    private void OnRightClick()
    {
        TweenFactory.RemoveTweenKey(ScrollTweenId, TweenStopBehavior.DoNotModify);

        var newPosition = _content.anchoredPosition.x - _itemContainerWidth - 1;

        var bound = _itemContainerWidth - _content.rect.width;
        if (newPosition < bound)
        {
            newPosition = bound;
        }

        ScrollTo(newPosition);
    }

    private void OnLeftClick()
    {
        TweenFactory.RemoveTweenKey(ScrollTweenId, TweenStopBehavior.DoNotModify);

        var newPosition = _content.anchoredPosition.x + _itemContainerWidth + 1;

        var bound = 0;
        if (newPosition > bound)
        {
            newPosition = bound;
        }

        ScrollTo(newPosition);
    }

    private void ScrollTo(float newPosition)
    {
        _content.gameObject.Tween(ScrollTweenId, _content.anchoredPosition, new Vector2(newPosition, _content.anchoredPosition.y), 0.5f, TweenScaleFunctions.CubicEaseOut, OnScrollTween);
    }

    private void OnScrollTween(ITween<Vector2> tween)
    {
        _content.anchoredPosition = tween.CurrentValue;
    }
}
