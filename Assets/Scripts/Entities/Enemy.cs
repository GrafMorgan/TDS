using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Player
{
    #region methods
    public Enemy(Transform transform, GameObject bulletPrefab, Transform shootSpawn) : base(transform, bulletPrefab, shootSpawn)
    {
        rechargeTimer = 100;
        speed = 4.5f;
        rotateSpeed = 2f;
    }

    public bool IsRecharge()
    {
        if (recharge == rechargeTimer - 1) return true;
        else return false;
    }

    public void StopRotation(float a)
    {
        transform.rotation = Quaternion.Euler(0, 0, a);
    }
    
    override public void RotateRight()
    {
        base.RotateRight();
    }

    override public void RotateLeft()
    {
        base.RotateLeft();
    }
    #endregion
}
