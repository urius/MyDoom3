using System;
using System.Collections.Generic;
using DigitalRuby.Tween;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentScrollView : MonoBehaviour
{
    public event Action<int> ItemMouseDown = delegate { };

    private const int ExtraCellsCount = 2;
    private const string ScollTweenKey = "ScrollTween";

    [SerializeField] private GameObject _scrollItemPrefab;
    [SerializeField] private GameObject _scrollContent;
    [SerializeField] private List<EquipmentSlotView> _slotViews;
    [SerializeField] private Collider2D _collider;
    public Collider2D Collider => _collider;
    [SerializeField] private Button _buttonLeft;
    [SerializeField] private Button _buttonRight;
    [SerializeField] private RectTransform _viewportRect;
    [SerializeField] private GridLayoutGroup _gridLayoutGroup;
    [SerializeField] private ScrollRect _scrollRect;

    private RectTransform _scrollContentRectTransform;
    private readonly List<Action> _disposeActions = new List<Action>();

    public void SetupItem(int index, EquipmentType equipmentType, Sprite iconSprite)
    {
        while (index >= _slotViews.Count)
        {
            CreateExtraCells(ExtraCellsCount);
        }

        _slotViews[index].SetSlotType(ToSlotType(equipmentType));
        _slotViews[index].SetSpriteIcon(iconSprite);

        SetItemVisibility(index, true);
    }

    public void ResetView(int index)
    {
        _slotViews[index].SetSlotType(EquipmentSlotType.Default);
        _slotViews[index].SetSpriteIcon(null);
    }

    public void SetItemVisibility(int itemIdex, bool visibility)
    {
        _slotViews[itemIdex].IconVisibility = visibility;
    }

    private void CreateExtraCells(int extraCellsCount)
    {
        var initialSlotViewsCount = _slotViews.Count;
        for (var i = initialSlotViewsCount; i < initialSlotViewsCount + extraCellsCount; i++)
        {
            var slotView = Instantiate(_scrollItemPrefab, _scrollContent.transform).GetComponent<EquipmentSlotView>();
            _slotViews.Add(slotView);

            var disposeAction = AddSlotListeners(i, _slotViews[i]);
            _disposeActions.Add(disposeAction);
        }
    }

    private void Awake()
    {
        _buttonLeft.onClick.AddListener(OnLeftClick);
        _buttonRight.onClick.AddListener(OnRightClick);

        _scrollContentRectTransform = _scrollContent.GetComponent<RectTransform>();
    }

    private void OnLeftClick()
    {
        var cellSize = _gridLayoutGroup.cellSize.x;

        var newPos = _scrollContentRectTransform.anchoredPosition.x + cellSize * 0.5f;
        newPos = Math.Min(0, (float)Math.Ceiling(newPos / cellSize) * cellSize);

        ScrollToOffset(newPos);
    }

    private void OnRightClick()
    {
        var cellSize = _gridLayoutGroup.cellSize.x;
        var minPos = _viewportRect.rect.width - _scrollContentRectTransform.rect.width;

        var newPos = _scrollContentRectTransform.anchoredPosition.x - cellSize * 0.5f;
        newPos = Math.Max(minPos, (float)Math.Floor(newPos / cellSize) * cellSize);

        ScrollToOffset(newPos);
    }

    private void ScrollToOffset(float newPos)
    {
        var starsPos = _scrollContentRectTransform.anchoredPosition;
        var targetPos = new Vector2(newPos, _scrollContentRectTransform.anchoredPosition.y);

        TweenFactory.RemoveTweenKey(ScollTweenKey, TweenStopBehavior.DoNotModify);
        _scrollContent.gameObject.Tween(ScollTweenKey, starsPos, targetPos, 0.5f, TweenScaleFunctions.CubicEaseOut, OnScrollContentTweenProgress);
    }

    private void OnScrollContentTweenProgress(ITween<Vector2> tween)
    {
        _scrollContentRectTransform.anchoredPosition = tween.CurrentValue;
    }

    private void OnEnable()
    {
        for (var i = 0; i < _slotViews.Count; i++)
        {
            var disposeAction = AddSlotListeners(i, _slotViews[i]);
            _disposeActions.Add(disposeAction);
        }
    }

    private void OnDisable()
    {
        TweenFactory.RemoveTweenKey(ScollTweenKey, TweenStopBehavior.DoNotModify);

        _disposeActions.ForEach(a => a());
        _disposeActions.Clear();
    }

    private Action AddSlotListeners(int i, EquipmentSlotView view)
    {
        void OnMouseDown()
        {
            ItemMouseDown(i);
        }
        view.OnMouseDown += OnMouseDown;

        return () => view.OnMouseDown -= OnMouseDown;
    }

    private EquipmentSlotType ToSlotType(EquipmentType equipmentType)
    {
        switch (equipmentType)
        {
            case EquipmentType.Engine:
                return EquipmentSlotType.Engine;
            case EquipmentType.Shield:
                return EquipmentSlotType.Shield;
            case EquipmentType.Weapon:
                return EquipmentSlotType.Weapon;
            default:
                throw new NotSupportedException($"slot type for equipment type {equipmentType} is not supported");
        }
    }
}
