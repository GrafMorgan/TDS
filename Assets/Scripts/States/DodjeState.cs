using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DodjeState : State
{
    #region variables
    private int newAngle,angle;
    private int timer;
    #endregion

    #region methods
    public static UnityEvent ChangeDodjeState = new UnityEvent();

    public DodjeState(Enemy en, StateMachine sm, Transform transform) : base(en, sm, transform)
    {

    }

    public override void Enter()
    {
        base.Enter();
        newAngle = (int)((20+Random.Range(0, 41))*Mathf.Pow(-1, Random.Range(1,3)));
        angle = 0;
        timer = -1;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        enemy.Stop();
        if (angle == newAngle)
        {
            if (timer == -1)
                timer = 35;
            else
            if (timer > 0) timer--;
            else
                ChangeDodjeState.Invoke();

            enemy.MoveBack();
        }
        else
        {
            if (angle > newAngle)
            {
                angle -= 1;
                enemy.RotateLeft();
            }
            else
            if (angle < newAngle)
            {
                angle += 1;
                enemy.RotateRight();
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
    #endregion
}
