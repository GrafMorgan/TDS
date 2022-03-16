using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootState : State
{
    private Transform hero;

    #region methods
    public ShootState(Enemy en, StateMachine sm, Transform transform, Transform hero) : base(en, sm, transform)
    {
        this.hero = hero;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        float angle1 = Angle(transform.position, transform.position + transform.up);
        float angle2 = Angle(transform.position, hero.position);

        enemy.Stop();
        if (Mathf.Abs(angle1 - angle2) <= 5)
        {
            enemy.StopRotation(180 - Vector2.SignedAngle(transform.position - hero.position, Vector2.up));
            dir = RotateDirection.none;
            enemy.Shoot();
        }
        else
        {
            
            if (angle1 != angle2 && dir == RotateDirection.none)
            {
                if (angle1 - angle2 < 0)
                {
                    dir = RotateDirection.right;
                    if (angle2 - angle1 > 180)
                        dir = RotateDirection.left;
                }
                else
                {
                    dir = RotateDirection.left;
                    if (angle1 - angle2 > 180)
                        dir = RotateDirection.right;
                }
            }
            if (dir == RotateDirection.left)
                enemy.RotateLeft();
            if (dir == RotateDirection.right)
                enemy.RotateRight();
        }
    }
    #endregion
}
