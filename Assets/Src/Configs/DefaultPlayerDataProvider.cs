using UnityEngine;

[CreateAssetMenu(fileName = "DefaultPlayerDataProvider", menuName = "Configs/DefaultPlayerDataProvider")]
public class DefaultPlayerDataProvider : ScriptableObject
{
    [SerializeField]
    private PlayerDataMin _playerData;

    public PlayerDataMin PlayerData => _playerData;
}
