using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BulletController : MonoBehaviour
{
    #region
    [SerializeField] private GameObject Internal;
    [SerializeField] private GameObject Extenal;

    private int timer, pTimer;
    #endregion

    #region methods
    public static UnityEvent EnemyScoreEvent = new UnityEvent();

    public static UnityEvent HeroScoreEvent = new UnityEvent();

    public void DeleteBullet()
    {
        timer = -1;
        Destroy(GetComponent<PolygonCollider2D>());
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(Extenal);
        Destroy(Internal);
        var ps = GetComponent<ParticleSystem>().emission;
        var emission = ps;
        emission.rateOverTime = 0;
        ps = emission;
        pTimer = 100;
    }
    #endregion

    #region MonoBehaviour
    void OnCollisionEnter2D(Collision2D col)
    {
        switch (col.gameObject.tag)
        {
            case "Enemy":
                HeroScoreEvent?.Invoke();
                DeleteBullet();
                break;
            case "Hero":
                EnemyScoreEvent?.Invoke();
                DeleteBullet();
                break;
            case "Bullet":
                col.gameObject.GetComponent<BulletController>().DeleteBullet();
                DeleteBullet();
                break;

        }
    }

    void Start()
    {
        timer = 500;
        pTimer = -1;
    }

    void FixedUpdate()
    {
        timer--;
        if (pTimer > 0) pTimer--;
        if (timer == 0)
            DeleteBullet();
        if (pTimer == 0)
        {
            Destroy(this.gameObject);
        }
    }
    #endregion
}
