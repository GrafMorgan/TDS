using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class ScoreManager : MonoBehaviour
{
    #region variables
    [SerializeField] private Text enemyText;
    [SerializeField] private Text heroText;
    private int heroScore, enemyScore;
    #endregion

    #region MonoBehaviour
    void Start()
    {
        heroScore = 0;
        enemyScore = 0;
        BulletController.EnemyScoreEvent.AddListener(EnemyScoreUp);
        BulletController.HeroScoreEvent.AddListener(HeroScoreUp);
    }
    #endregion

    #region methods
    private void EnemyScoreUp()
    {
        enemyScore++;
        enemyText.text = enemyScore.ToString();
    }
    private void HeroScoreUp()
    {
        heroScore++;
        heroText.text = heroScore.ToString();
    }
    #endregion

}
