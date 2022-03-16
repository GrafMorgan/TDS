using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphEdge : MonoBehaviour
{
    #region variables
    [SerializeField] private bool isHero = false, isEnemy = false;
    [SerializeField] public int edgeWeight;
    public enum Direction { left, right, top, bottom, none };
    [SerializeField] private Direction direction;
    [SerializeField] private GraphEdge leftEdge, rightEdge, topEdge, bottomEdge;
    #endregion

    #region methods
    public void SearchWay(int x, int y)
    {
        if (leftEdge != this) if (x>0)
        {
            if(leftEdge.GetWeight() > edgeWeight+1)
            {
                leftEdge.SetWeight(edgeWeight + 1);
                leftEdge.SetDirection(Direction.right);
                leftEdge.SearchWay(x - 1, y);
            }
        }
        if (rightEdge != this) if (x<20)
        {
            if(rightEdge.GetWeight() > edgeWeight+1)
            {
                rightEdge.SetWeight(edgeWeight + 1);
                rightEdge.SetDirection(Direction.left);
                rightEdge.SearchWay(x + 1, y);
            }
        }
        if(topEdge != this) if (y > 0)
        {
            if(topEdge.GetWeight() > edgeWeight + 1)
                {
                    topEdge.SetWeight(edgeWeight + 1);
                    topEdge.SetDirection(Direction.bottom);
                    topEdge.SearchWay(x, y-1);
                }
        }
        if (bottomEdge != this) if (y < 9)
        {
            if (bottomEdge.GetWeight() > edgeWeight + 1)
                {
                    bottomEdge.SetWeight(edgeWeight + 1);
                    bottomEdge.SetDirection(Direction.top);
                    bottomEdge.SearchWay(x, y+1);
                }
        }
    }

    public void InitializeEdge(GraphEdge lEdge, GraphEdge rEdge, GraphEdge tEdge, GraphEdge bEdge)
    {
        leftEdge = lEdge;
        rightEdge = rEdge;
        topEdge = tEdge;
        bottomEdge = bEdge;
    }

    private void MakeDeadEnd(bool left, bool right, bool top, bool bottom)
    {
        if (left) leftEdge = this;
        if (right) rightEdge = this;
        if (top) topEdge = this;
        if (bottom) bottomEdge = this;
    }

    public void Delete()
    {
        rightEdge.MakeDeadEnd(true, false, false, false);
        leftEdge.MakeDeadEnd(false, true, false, false);
        bottomEdge.MakeDeadEnd(false, false, true, false);
        topEdge.MakeDeadEnd(false, false, false, true);
        Destroy(this.gameObject);
    }

    public bool IsHero()
    {
        return isHero;
    }

    public bool IsEnemy()
    {
        return isEnemy;
    }

    public int GetWeight()
    {
        return edgeWeight;
    }

    public void SetWeight(int w)
    {
        edgeWeight = w;
    }

    public void SetDirection(string dir)
    {
        switch(dir)
        {
            case "left":
                direction = Direction.left;
                break;
            case "right":
                direction = Direction.right;
                break;
            case "top":
                direction = Direction.top;
                break;
            case "bottom":
                direction = Direction.bottom;
                break;
            case "none":
                direction = Direction.none;
                break;
        }
    }

    public GraphEdge GetDirection()
    {
        switch (direction)
        {
            case Direction.left:
                return leftEdge;
                break;
            case Direction.right:
                return rightEdge;
                break;
            case Direction.top:
                return topEdge;
                break;
            case Direction.bottom:
                return bottomEdge;
                break;
            default:
                return this;
                break;
        }
    }

    public void SetDirection(Direction dir)
    {
        direction = dir;
    }
    #endregion

    #region MonoBehaviour
    public void OnTriggerEnter2D(Collider2D col)
    {
        switch (col.gameObject.tag)
        {
            case "Wall":
                Delete();
                break;
            case "Hero":
                isHero = true;
                break;
            case "Enemy":
                isEnemy = true;
                break;
        }
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        switch (col.gameObject.tag)
        {
            case "Hero":
                isHero = false;
                break;
            case "Enemy":
                isEnemy = false;
                break;
        }
    }
    #endregion
}
