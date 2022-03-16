using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    #region variables
    [SerializeField] private LineRenderer line;
    [SerializeField] private KeyCode moveForward, moveBack, rotateLeft, rotateRight, shootKey;
    [SerializeField] private GameObject bulletPrefab;
    private Hero hero;
    #endregion

    #region MonoBehaviour
    void Start()
    {
        hero = new Hero(GetComponent<Transform>(),bulletPrefab,transform.Find("ShootSpawn"));
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Wall")
            hero.Stop();
    }

    void FixedUpdate()
    {
        Vector3 pos1 = transform.Find("ShootSpawn").position;
        Vector3 pos2 = transform.Find("ShootSpawn").position + transform.up;
        TrajectoryRenderer.TrajectoryRender(pos1, pos2, 30,0, null, line);
        hero.Recharge();
        if (Input.GetKey(moveForward))
            hero.MoveForward();
        else if (Input.GetKey(moveBack))
            hero.MoveBack();
        else hero.Stop();
        if (Input.GetKey(rotateLeft))
            hero.RotateLeft();
        else if (Input.GetKey(rotateRight))
            hero.RotateRight();
        if (Input.GetKey(shootKey))
            hero.Shoot();
    }
    #endregion
}
