using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    #region variables
    [SerializeField] private KeyCode eCode;
    [SerializeField] private KeyCode qCode;
    [SerializeField] private KeyCode rCode;
    #endregion

    #region methods
    public static UnityEvent IsDrawGraph = new UnityEvent();
    #endregion

    #region MonoBehaviour
    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(rCode))
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        if (Input.GetKeyDown(eCode))
            IsDrawGraph.Invoke();
        if (Input.GetKeyDown(qCode))
            Application.Quit();

    }
    #endregion
}
