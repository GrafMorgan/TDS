using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryRenderer : MonoBehaviour
{

    static public void TrajectoryRender(Vector3 pos1, Vector3 pos2, float distance, int num, GameObject last, LineRenderer line)
    {
        if (num == 0)
        {
            line.positionCount = 1;
            line.SetPosition(0, pos1);

        }
        if (num < 3)
        {
            RaycastHit2D[] hits;
            Ray ray = new Ray(pos1, (pos2 - pos1) / distance);

            hits = Physics2D.RaycastAll(ray.origin, ray.direction, distance);
            if (hits != null)
            {
                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.collider.gameObject.tag == "Wall" && hit.collider.gameObject != last)
                    {
                        line.positionCount += 1;
                        line.SetPosition(line.positionCount - 1, hit.point);
                        Vector2 inReflection = Vector2.Reflect(ray.direction, hit.normal);
                        TrajectoryRender(hit.point, hit.point + inReflection, distance, num + 1, hit.collider.gameObject, line);
                        break;
                    }
                }
            }
        }
        else
        {
            line.positionCount += 1;
            line.SetPosition(line.positionCount - 1, pos2);

        }
    }

}
