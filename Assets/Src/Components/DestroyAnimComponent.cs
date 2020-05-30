using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class DestroyAnimComponent : MonoBehaviour
{
    [SerializeField]
    private Transform[] _animParts;

    private Quaternion[] _rotationSpeeds;
    private Vector3[] _flySpeeds;
    private TaskCompletionSource<bool> _endAnimationTcs;

    public Task EndAnimationTask => _endAnimationTcs.Task;

    private void OnEnable()
    {
        _endAnimationTcs = new TaskCompletionSource<bool>();

        _rotationSpeeds = _animParts.Select(_ => GetRandomRotation()).ToArray();
        var centerPartPosition = _animParts[0].position;
        _flySpeeds = _animParts.Select(p => GetFlySpeed(p, centerPartPosition)).ToArray();
    }

    private void Update()
    {
        Transform tempTransform;
        for (var i = 0; i < _animParts.Length; i++)
        {
            tempTransform = _animParts[i].transform;
            tempTransform.position += _flySpeeds[i];
            tempTransform.rotation = _rotationSpeeds[i] * tempTransform.rotation;
        }
    }

    private Vector3 GetFlySpeed(Transform part, Vector3 centerPartPosition)
    {
        return (part.position - centerPartPosition).normalized + (0.5f * Random.insideUnitSphere);
    }

    private Quaternion GetRandomRotation()
    {
        return Quaternion.AngleAxis(Random.Range(-2, 2), Random.insideUnitSphere);
    }
}
