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
    public void OnMouseUp()
    {
        Debug.Log("GameStartButton");
        StageLoadManager.GetInstance().LoadStage_Infinite();
        if (StageLoadManager.GetInstance().isStageLoaded() == true)
            StartCoroutine(StartScene());
        else
            Debug.LogError("Stage Load Failed!!!!");
    }

    IEnumerator StartScene()
    {
        yield return null;
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync("GameScene");

        while(!loadOperation.isDone)
        {
            Debug.Log(loadOperation.progress);

            yield return null;
        }
    }
}
