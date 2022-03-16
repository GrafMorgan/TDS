using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour
{
    #region variables
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private LineRenderer line;
    private Enemy enemy;
    [SerializeField] private GameObject hero;
    private Transform transform;
    private StateMachine stateMachine;
    private SearchState searchState;
    private ShootState shootState;
    private DodjeState dodjeState;
    #endregion

    #region methods
    public static UnityEvent FindPath = new UnityEvent();

    private void ChangeDodjeState()
    {
        Vector3 pos1 = transform.position;
        Vector3 pos2 = hero.GetComponent<Transform>().position;
        RaycastHit2D[] hits = Physics2D.RaycastAll(pos1, (pos2 - pos1) /
        Vector3.Distance(pos2, pos1), Vector3.Distance(pos2, pos1));
        bool isWallCollidered = false;
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.tag == "Wall")
                isWallCollidered = true;
        }
        if (!isWallCollidered)
        {
            if (Vector3.Distance(pos1, pos2) < 10)
                stateMachine.ChangeState(shootState);
        }
        else
        {
            stateMachine.ChangeState(searchState);
        }
        stateMachine.ChangeState(searchState);
    }
    #endregion

    #region MonoBehaviour
    void Start()
    {
        DodjeState.ChangeDodjeState.AddListener(ChangeDodjeState);
        transform = GetComponent<Transform>();
        enemy = new Enemy(transform, bulletPrefab, transform.Find("ShootSpawn"));
        stateMachine = new StateMachine();
        searchState = new SearchState(enemy, stateMachine, transform);
        shootState = new ShootState(enemy, stateMachine, transform, hero.GetComponent<Transform>());
        dodjeState = new DodjeState(enemy, stateMachine, transform);
        stateMachine.Initialize(searchState);

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Wall")
        {
            FindPath.Invoke();
        }
    }

    void FixedUpdate()
    {

        Vector3 pos1 = transform.position;
        Vector3 pos2 = hero.GetComponent<Transform>().position;
        RaycastHit2D[] hits = Physics2D.RaycastAll(pos1, (pos2 - pos1) /
        Vector3.Distance(pos2, pos1), Vector3.Distance(pos2, pos1));
        bool isWallCollidered = false;
        foreach(RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.tag == "Wall")
                isWallCollidered = true;
        }
        if (!isWallCollidered)
        {
            if(Vector3.Distance(pos1, pos2)<10)
                if(stateMachine.currentState==searchState)stateMachine.ChangeState(shootState);
        }
        else
        {
            if (stateMachine.currentState != searchState) stateMachine.ChangeState(searchState);
        }
        if(enemy.IsRecharge() == true && stateMachine.currentState != dodjeState)
        {
            if(Random.Range(1,10)<4)
            {
                stateMachine.ChangeState(dodjeState);
            }
        }
        stateMachine.currentState.LogicUpdate();
    }
    #endregion
}
