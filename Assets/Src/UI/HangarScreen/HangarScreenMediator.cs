using UnityEngine;
using Zenject;

public class HangarScreenMediator : MonoBehaviour
{
    [SerializeField]
    private GameObject _flyingEquipmentPrefab;
    [SerializeField]
    private Transform _flyingEquipmentParent;

    private MenuEventsAggregator _menuEventsAggregator;

    private FlyingEquipmentView _flyingEquipmentView;
    private EquipmentConfigBase _flyingEquipmentData;
    private Camera _mainCamera;
    private float _screenZ;

    [Inject]
    private void Construct(MenuEventsAggregator menuEventsAggregator)
    {
        _menuEventsAggregator = menuEventsAggregator;
    }

    private void OnEnable()
    {
        _mainCamera = Camera.main;
        _screenZ = transform.position.z - _mainCamera.transform.position.z;

        _menuEventsAggregator.EquipmentSlotMouseDown += OnEquipmentSlotMouseDown;
    }

    private void OnDisable()
    {
        _menuEventsAggregator.EquipmentSlotMouseDown -= OnEquipmentSlotMouseDown;
    }

    private void OnEquipmentSlotMouseDown(EquipmentConfigBase equipment)
    {
        if (equipment != null && _flyingEquipmentView == null)
        {
            _flyingEquipmentData = equipment;
            _flyingEquipmentView = Instantiate(_flyingEquipmentPrefab, _flyingEquipmentParent).GetComponent<FlyingEquipmentView>();
            _flyingEquipmentView.SetIconSprite(equipment.IconSprite);
        }
    }

    void Update()
    {
        var isMouseReleased = Input.GetMouseButtonUp(0);
        if (_flyingEquipmentView != null)
        {
            var point = _mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenZ));
            _flyingEquipmentView.transform.position = point;

            if (isMouseReleased)
            {
                if (_flyingEquipmentView != null)
                {

                    Destroy(_flyingEquipmentView.gameObject);
                    _flyingEquipmentView = null;

                    _menuEventsAggregator.FlyingEquipmentMouseUp(_flyingEquipmentData, point);
                    _flyingEquipmentData = null;
                }
            }
        }

        if (isMouseReleased)
        {
            _menuEventsAggregator.MouseUp();
        }
    }
}
