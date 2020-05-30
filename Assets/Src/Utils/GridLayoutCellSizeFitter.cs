using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class GridLayoutCellSizeFitter : MonoBehaviour
{
    [SerializeField]
    private GridLayoutGroup _gridLayoutGroup;
    [SerializeField]
    private RectTransform _contentRectTransform;
    [SerializeField]
    private Vector2 _cellSizeProportion;

    private void Start()
    {
        UpdateCellSize();
    }

    private void Update()
    {
        UpdateCellSize();
    }

    private void UpdateCellSize()
    {
        var rect = _contentRectTransform.rect;
        _gridLayoutGroup.cellSize = new Vector2(rect.width * _cellSizeProportion.x, rect.height * _cellSizeProportion.y);
    }
}
