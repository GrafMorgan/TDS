using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region variables
    protected float speed, rotateSpeed;
    protected Transform transform;
    protected Rigidbody2D rb;
    protected GameObject bulletPrefab;
    protected Transform shootSpawn;
    protected float shootForce = 3f;
    protected int recharge = 0;
    protected int rechargeTimer = 30;
    #endregion

    #region methods
    public Player(Transform transform, GameObject bulletPrefab, Transform shootSpawn)
    {
        this.transform = transform;
        rb = transform.gameObject.GetComponent<Rigidbody2D>();
        this.bulletPrefab = bulletPrefab;
        this.shootSpawn = shootSpawn;
        speed = 5;
        rotateSpeed = 3;
    }
    public void MoveForward()
    {
        rb.velocity = transform.up * speed;
            //rb.AddForce(transform.up * speed);
    }
    public void MoveBack()
    {
        rb.velocity = -transform.up * speed;
    }
    virtual public void RotateLeft()
    {
        transform.Rotate(0, 0, rotateSpeed);
        rb.velocity = Quaternion.AngleAxis(rotateSpeed, new Vector3(0,0,1))*rb.velocity;
    }
    virtual public void RotateRight()
    {
        transform.Rotate(0, 0, -rotateSpeed);
        rb.velocity = Quaternion.AngleAxis(-rotateSpeed, new Vector3(0, 0, 1)) * rb.velocity;
    }
    public void Shoot()
    {
        if (recharge == 0)
        {
            recharge = rechargeTimer;
            GameObject bull = Instantiate(bulletPrefab, shootSpawn.position, transform.rotation);
            bull.GetComponent<Rigidbody2D>().AddForce(transform.up, ForceMode2D.Impulse);
        }
    }
    public void Recharge()
    {
        if (recharge > 0) recharge--;
    }
    public void Stop()
    {
        rb.velocity = Vector3.zero;
    }
    #endregion
}
