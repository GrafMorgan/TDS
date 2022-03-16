using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region variables
    [SerializeField] private Transform heroTransform;
    private Transform transform;
    #endregion

    #region MonoBehaviour
    void Start()
    {
        transform = GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        Vector3 a = heroTransform.position;
        a.z = transform.position.z;
        transform.position = a;
    }
    #endregion
}
