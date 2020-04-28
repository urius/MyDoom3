using UnityEngine;
using Zenject;

public class EnemyShipsController : ITickable
{
    private ScreenBoundsProvider _screenBoundsProvider;
    private IEnemyShipModelsProvider _enemyShipModelsProvider;
    private IPlayerShipModelProvider _playerShipModelProvider;

    [Inject]
    public void Construct(
        ScreenBoundsProvider screenBoundsProvider,
        IEnemyShipModelsProvider enemyShipModelsProvider,
        IPlayerShipModelProvider playerShipModelProvider)
    {
        _enemyShipModelsProvider = enemyShipModelsProvider;
        _playerShipModelProvider = playerShipModelProvider;
        _screenBoundsProvider = screenBoundsProvider;
    }

    public void Tick()
    {
        var i = 0;
        while (i < _enemyShipModelsProvider.Models.Count)
        {
            var enemyShip = _enemyShipModelsProvider.Models[i];
            if (ProcessBounds(enemyShip))
            {
                _enemyShipModelsProvider.Models.RemoveAt(i);
                enemyShip.Dispose();
                continue;
            }
            enemyShip.UpdateCooldowns();
            UpdateEnemyShipBehaviour(enemyShip, _playerShipModelProvider.ShipModel);
            i++;
        }
    }

    private bool ProcessBounds(EnemyShipModel enemyShip)
    {
        return enemyShip.Position.z < _screenBoundsProvider.Bounds.center.y - _screenBoundsProvider.Bounds.height * 2;
    }

    private void UpdateEnemyShipBehaviour(EnemyShipModel enemyShip, ShipModel playerShip)
    {
        if (enemyShip.AIState is AIFlyForwardState flyForward)
        {
            ProcessFlyStraightState(enemyShip);
        }
        else if (enemyShip.AIState is AIMoveToTargetState moveToTarget)
        {
            ProcessMoveToTargetState(moveToTarget, enemyShip, playerShip.Position);
            enemyShip.FireAllWeapons();
        }
        else if (enemyShip.AIState is AIMoveFromTargetState moveFromTarget)
        {
            ProcessMoveFromTargetState(moveFromTarget, enemyShip);
        }

        enemyShip.Position += enemyShip.Forward.normalized * enemyShip.Speed;
    }

    private void ProcessMoveToTargetState(AIMoveToTargetState moveToTarget, EnemyShipModel enemyShip, Vector3 userShipPosition)
    {
        var deltaPosition = userShipPosition - enemyShip.Position;
        if (deltaPosition.magnitude < moveToTarget.SwitchStateDistance
            || enemyShip.Position.z < userShipPosition.z - moveToTarget.SwitchStateDistance)
        {
            var targetDirection = new Vector3(enemyShip.Position.x < userShipPosition.x ? -1 : 1, 0, -UnityEngine.Random.value * 3 - 1);
            enemyShip.AIState = new AIMoveFromTargetState(Quaternion.LookRotation(targetDirection, Vector3.up));
            return;
        }

        var targetRotation = Quaternion.LookRotation(deltaPosition, Vector3.up);
        RotateTo(enemyShip, targetRotation);
    }

    private void ProcessFlyStraightState(EnemyShipModel enemyShip)
    {
        if (enemyShip.Position.z < _screenBoundsProvider.Bounds.yMax)
        {
            enemyShip.AIState = new AIMoveToTargetState();
        }
    }

    private void ProcessMoveFromTargetState(AIMoveFromTargetState moveFromTarget, EnemyShipModel enemyShip)
    {
        RotateTo(enemyShip, moveFromTarget.TargetRotation);
    }

    private void RotateTo(EnemyShipModel enemyShip, Quaternion targetRotation)
    {
        enemyShip.Rotation = Quaternion.RotateTowards(enemyShip.Rotation, targetRotation, enemyShip.RotationSpeed);
    }
}
