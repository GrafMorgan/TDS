using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State 
{
    #region variables
    protected Enemy enemy;
    protected StateMachine stateMachine;
    protected Transform transform;
    #endregion

    #region methods
    protected enum RotateDirection { none, left, right, stop };
    protected RotateDirection dir = RotateDirection.none;

    public virtual void Enter()
    {

    }

    protected float Angle(Vector3 pos1, Vector3 pos2)
    {
        float a = Vector2.SignedAngle(pos1 - pos2, Vector3.up);
        return a;
    }

    protected State(Enemy en, StateMachine sm, Transform transform)
    {
        enemy = en;
        stateMachine = sm;
        this.transform = transform;
    }

    public virtual void LogicUpdate()
    {
        enemy.Recharge();
    }


    public virtual void Exit()
    {

    }
    #endregion
}
