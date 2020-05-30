using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapShipsAnimator : MonoBehaviour
{
    private enum AppearingSide
    {
        Right,
        Left,
    }

    private class AnimationSetting
    {
        public readonly AppearingSide AppearingSide;
        public readonly GameObject Prefab;

        public AnimationSetting(AppearingSide appearingSide, GameObject prefab)
        {
            AppearingSide = appearingSide;
            Prefab = prefab;
        }
    }

    private const float SwitchAnimationDuration = 0.6f;

    [SerializeField]
    private Transform[] _shipHolders;
    private Transform[] _shipViews;
    private GameObject[] _shipPrefabs;

    [SerializeField]
    private Transform _leftPos;
    [SerializeField]
    private Transform _centerPos;
    [SerializeField]
    private Transform _rightPos;

    private readonly Queue<AnimationSetting> _animationsQueue = new Queue<AnimationSetting>();

    private AnimationSetting _currentAnimation;
    private int _currentShipHolderIndex = 0;
    private float _startAnimationTime;

    public event Action<int> SwitchAnimationStarted = delegate { };

    void Awake()
    {
        _shipViews = new Transform[_shipHolders.Length];
        _shipPrefabs = new GameObject[_shipHolders.Length];
    }

    public void ShowCurrentPrefab(GameObject prefab)
    {
        ShowPrefabOnIndex(_currentShipHolderIndex, prefab);
    }

    public void ShowPrefabFromRight(GameObject prefab)
    {
        _animationsQueue.Enqueue(new AnimationSetting(AppearingSide.Right, prefab));
    }

    public void ShowPrefabFromLeft(GameObject prefab)
    {
        _animationsQueue.Enqueue(new AnimationSetting(AppearingSide.Left, prefab));
    }

    private void Update()
    {
        var appearingHolderIndex = 1 - _currentShipHolderIndex;
        if (_currentAnimation == null)
        {
            if (_animationsQueue.Count > 0)
            {
                _currentAnimation = _animationsQueue.Dequeue();

                _shipHolders[appearingHolderIndex].transform.position = _currentAnimation.AppearingSide == AppearingSide.Right ? _rightPos.position : _leftPos.position;

                ShowPrefabOnIndex(appearingHolderIndex, _currentAnimation.Prefab);

                _startAnimationTime = Time.time;

                SwitchAnimationStarted(appearingHolderIndex);
            }
            else
            {
                return;
            }
        }

        if (_currentAnimation != null)
        {
            var targetPosition = _currentAnimation.AppearingSide == AppearingSide.Right ? _leftPos.position : _rightPos.position;
            var interpolationValue = (Time.time - _startAnimationTime) / SwitchAnimationDuration;
            _shipHolders[_currentShipHolderIndex].position = Vector3.Slerp(_shipHolders[_currentShipHolderIndex].position, targetPosition, interpolationValue);
            _shipHolders[appearingHolderIndex].position = Vector3.Slerp(_shipHolders[appearingHolderIndex].position, _centerPos.position, interpolationValue);

            if (Vector3.Distance(_shipHolders[appearingHolderIndex].position, _centerPos.position) < 0.5f)
            {
                _shipHolders[appearingHolderIndex].position = _centerPos.position;
                _currentShipHolderIndex = appearingHolderIndex;
                _currentAnimation = null;
            }
        }
    }

    private void ShowPrefabOnIndex(int index, GameObject prefab)
    {
        if (_shipPrefabs[index] != prefab)
        {
            _shipPrefabs[index] = prefab;
            if (_shipViews[index] != null)
            {
                Destroy(_shipViews[index].gameObject);
            }

            _shipViews[index] = Instantiate(prefab, _shipHolders[index].transform).transform;
        }
    }
}
