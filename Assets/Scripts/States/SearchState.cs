using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SearchState : State
{
    #region variables
    private Vector3[] path;
    private int numOfPoint = 1;
    #endregion

    #region methods
    public static UnityEvent FindPath = new UnityEvent();

    public SearchState(Enemy en, StateMachine sm, Transform transform) : base(en, sm, transform)
    {
        GraphController.EnemyNewPath.AddListener(NewPath);
    }


    private void NewPath(Vector3[] NewPath)
    {
        bool a = false;
        if (path != null)
        {
            for (int i = 0; i < NewPath.Length; i++)
            {
                if (NewPath[i] == path[numOfPoint])
                {
                    numOfPoint = i;
                    a = true;
                }
            }
        }
        path = NewPath;
        if (!a)
        {
            numOfPoint = path.Length - 2;
            dir = RotateDirection.none;
        }
    }

    public override void Enter()
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (path != null)
        {
            float angle1 = Angle(transform.position, transform.position + transform.up);
            float angle2 = Angle(transform.position, path[numOfPoint]);

                if (Mathf.Abs(angle1 - angle2) <= 5)
                {
                    enemy.StopRotation(180 - Vector2.SignedAngle(transform.position - path[numOfPoint], Vector2.up));
                    dir = RotateDirection.none;
                    enemy.MoveForward();
                    if (Mathf.Abs(Mathf.Abs(transform.position.x) - Mathf.Abs(path[numOfPoint].x)) +
                        Mathf.Abs(Mathf.Abs(transform.position.y) - Mathf.Abs(path[numOfPoint].y)) < 0.5)
                    {
                        if (numOfPoint > 0)
                            numOfPoint--;
                        else
                        {
                            path = null;
                            dir = RotateDirection.stop;
                            FindPath.Invoke();
                        }
                    }
                }
                else
                {
                enemy.Stop();
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

    }

    public override void Exit()
    {
        base.Exit();
        enemy.Stop();
    }
    #endregion
}
