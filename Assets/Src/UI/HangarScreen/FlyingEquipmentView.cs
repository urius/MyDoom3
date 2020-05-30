using UnityEngine;
using UnityEngine.UI;

public class FlyingEquipmentView : MonoBehaviour
{
    [SerializeField]
    private Image _image;

    public void SetIconSprite(Sprite iconSprite)
    {
        _image.sprite = iconSprite;
    }
}
