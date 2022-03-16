using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GraphController : MonoBehaviour
{


    #region const
    const int sizeX = 21, sizeY = 10;
    #endregion

    #region variables
    private bool isDraw = false;
    private int isGraphUpdate = 0;
    private bool isInterfaceDraw = true;
    [SerializeField]  private bool isHeroDetected = false, isEnemyDetected = false;
    [SerializeField] GameObject edgePrefab;
    [SerializeField] LineRenderer line;
    private Transform transform;
    private int widthOfWay;
    private Vector3 pos;
    [SerializeField] private int heroX, heroY, enemyX, enemyY;
    [SerializeField] private Vector3[] path;
    private GraphEdge[,] graph = new GraphEdge[sizeX,sizeY];
    #endregion

    #region methods
    public static UnityEvent<Vector3[]> EnemyNewPath = new UnityEvent<Vector3[]>();

    private void IsDrawGraph()
    {
        isInterfaceDraw = !isInterfaceDraw;
        line.positionCount = 0;
    }

    private void DrawGraph()
    {
        line.positionCount = 0;
        line.positionCount = path.Length;
        line.SetPositions(path);
        isDraw = false;
    }

    private void PathSelection()
    {
        if (graph[heroX, heroY].GetWeight() > 0)
        {
            path = new Vector3[graph[heroX, heroY].GetWeight() + 1];
            GraphEdge a = graph[heroX, heroY];
            int d = 0;
            int t = 0;
            while (true)
            {
                path[t] = a.gameObject.GetComponent<Transform>().position;
                a = a.GetDirection();
                t++;
                if (t == path.Length) break;
            }
            CutWay();
            EnemyNewPath?.Invoke(path);
        }
    }

    private void GenerateGraph()
    {
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                Vector3 newPos = pos;
                newPos.x += i * 2;
                newPos.y -= j * 2; ;
                GameObject a = Instantiate(edgePrefab, newPos, new Quaternion(0, 0, 0, 0), transform);
                graph[i, j] = a.GetComponent<GraphEdge>();

            }
        }
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                GraphEdge lEdge, rEdge, tEdge, bEdge;
                if (j == 0) tEdge = graph[i, j]; else tEdge = graph[i, j - 1];
                if (j == sizeY - 1) bEdge = graph[i, j]; else bEdge = graph[i, j + 1];
                if (i == 0) lEdge = graph[i, j]; else lEdge = graph[i - 1, j];
                if (i == sizeX - 1) rEdge = graph[i, j]; else rEdge = graph[i + 1, j];
                graph[i, j].InitializeEdge(lEdge, rEdge, tEdge, bEdge);

            }
        }

    }

    private void NormalizeWay()
    {
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                if (graph[i, j] != null)
                {
                    graph[i, j].SetWeight(99999999);
                    graph[i, j].SetDirection("none");
                }
            }
        }
    }

    public void CutWay()
    {
        int currentPointNum = 0;
        int num = 0;
        int sizeOfPoints = 1;
        bool[] hitsNum = new bool[path.Length];
        for (int i = 0; i < hitsNum.Length; i++) hitsNum[i] = false;
        hitsNum[0] = true;
        bool isLast = false;
        int ao = 0;
        while (!isLast)
        {
            ao++;
            bool isWallCollidered = false;
            num++;
            RaycastHit2D[] hits;
            hits = Physics2D.RaycastAll(path[currentPointNum], (path[num] - path[currentPointNum]) / Vector3.Distance(path[num], path[currentPointNum]), Vector3.Distance(path[num], path[currentPointNum]));
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.gameObject.tag == "Wall") isWallCollidered = true;
            }
            if (isWallCollidered)
            {
                sizeOfPoints++;
                hitsNum[num - 1] = true;
                currentPointNum = num - 1;
            }
            if (num == path.Length - 1)
            {
                isLast = true;
                hitsNum[num] = true;
                sizeOfPoints++;
            }
        }
        Vector3[] newWay = new Vector3[sizeOfPoints];
        for (int i = 0, j = 0; i < path.Length; i++)
        {
            if (hitsNum[i])
            {
                newWay[j] = path[i];
                j++;
            }
        }
        path = newWay;
        isDraw = true;
    }

    public void SearchPlayers()
    {
        isHeroDetected = false;
        isEnemyDetected = false;
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                if (!isHeroDetected)
                {
                    if (graph[i, j].IsHero())
                    {
                        if (i != heroX || j != heroY)
                        heroX = i; heroY = j;
                        isHeroDetected = true;
                    }
                }
                if (!isEnemyDetected)
                {
                    if (graph[i, j].IsEnemy())
                    {
                        if (i != enemyX || j != enemyY)
                        enemyX = i;
                        enemyY = j;
                        isEnemyDetected = true;
                    }
                }
            }
        }
    }

    public void SearchWay()
    {
            NormalizeWay();
            graph[enemyX, enemyY].SetWeight(0);
            graph[enemyX, enemyY].SearchWay(enemyX, enemyY);
            PathSelection();
    }
    #endregion

    #region MonoBehaviour
    void Start()
    {
        transform = GetComponent<Transform>();
        SearchState.FindPath.AddListener(SearchWay);
        EnemyController.FindPath.AddListener(SearchWay);
        GameManager.IsDrawGraph.AddListener(IsDrawGraph);
        pos = transform.position;
        GenerateGraph();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        SearchPlayers();
        if (isGraphUpdate == 0 && isHeroDetected && isEnemyDetected)
        {
            SearchWay();
            isGraphUpdate = 100;
        }
        if (isGraphUpdate > 0) isGraphUpdate--;
        if (isDraw && isInterfaceDraw) DrawGraph();
    }
    #endregion
}
