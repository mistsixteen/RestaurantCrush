using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneUI : MonoBehaviour
{

    private Text scoreText, moveText;
    private Text targetText;
    private Text gameClearText, gameOverText;

    public static GameSceneUI instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = GetComponent<GameSceneUI>();
        scoreText = GameObject.Find("CurrentScore").GetComponent<Text>();
        moveText = GameObject.Find("MoveLeft").GetComponent<Text>();

        gameOverText = GameObject.Find("GameOverUI").GetComponent<Text>();
        gameOverText.gameObject.SetActive(false);
        gameClearText = GameObject.Find("GameClearUI").GetComponent<Text>();
        gameClearText.gameObject.SetActive(false);

        targetText = GameObject.Find("TargetText").GetComponent<Text>();
        targetText.gameObject.SetActive(false);
    }

    public void SetObjectUI(StageInfo currentStageInfo)
    {
        if(currentStageInfo.CurrentStageType != StageType.StageTypeInfinte)
        {
            targetText.gameObject.SetActive(true);
        }
    }

    public void UpdateUI(StageInfo currentStageInfo)
    {
        string fmt = "00000000";
        scoreText.text = currentStageInfo.Score.ToString(fmt);
        moveText.text = currentStageInfo.MoveLeft.ToString(fmt);

        if (currentStageInfo.CurrentStageType == StageType.StageTypeTargetScore)
        {
            targetText.text = "Target Score : \n" + currentStageInfo.targetScore.ToString(fmt);
        }

    }

    public void GameOverScreen()
    {
        gameOverText.gameObject.SetActive(true);
    }
    public void GameClearScreen(StageInfo currentStageInfo)
    {
        string fmt = "00000000";
        gameClearText.text = "Game Clear!!!!\nScore : " + currentStageInfo.targetScore.ToString(fmt);
        gameClearText.gameObject.SetActive(true);
    }

}
