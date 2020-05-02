using UnityEngine;

public class EnemyShipModel : ShipModel
{
    public AIStateBase AIState;

    public EnemyShipModel(ShipData shipData)
        : base(TeamId.Enemy, shipData)
    {
        AIState = new AIFlyForwardState();
    }
}

public abstract class AIStateBase { }

public class AIFlyForwardState : AIStateBase { }

public class AIMoveToTargetState : AIStateBase
{
    public readonly int SwitchStateDistance = 40;
}

public class AIMoveFromTargetState : AIStateBase
{
    public readonly Quaternion TargetRotation;

    public AIMoveFromTargetState(Quaternion targetRotation)
    {
        TargetRotation = targetRotation;
    }
}

