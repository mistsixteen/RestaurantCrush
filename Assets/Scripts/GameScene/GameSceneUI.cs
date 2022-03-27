using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneUI : MonoBehaviour
{

    private Text scoreText, moveText, gameOverText;


    public static GameSceneUI instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = GetComponent<GameSceneUI>();
        scoreText = GameObject.Find("CurrentScore").GetComponent<Text>();
        moveText = GameObject.Find("MoveLeft").GetComponent<Text>();
        gameOverText = GameObject.Find("GameOverUI").GetComponent<Text>();
        gameOverText.gameObject.SetActive(false);
    }

    public void UpdateUI(StageInfo currentStageInfo)
    {
        string fmt = "00000000";
        scoreText.text = currentStageInfo.Score.ToString(fmt);
        moveText.text = currentStageInfo.MoveLeft.ToString(fmt);
    }

    public void GameOverScreen()
    {
        gameOverText.gameObject.SetActive(true);
    }

}
