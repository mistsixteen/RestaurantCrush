using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnMouseDown()
    {
        Debug.Log("GameStartButton");
        StageLoadManager.GetInstance().LoadStage_Infinite();
        if (StageLoadManager.GetInstance().isStageLoaded() == true)
            SceneManager.LoadScene("InfiniteModeScene");
        else
            Debug.LogError("Stage Load Failed!!!!");
    }
}
