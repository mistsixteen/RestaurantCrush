using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InfiniteModeButton : MonoBehaviour
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
        Debug.Log("InfiniteModeButton");
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

        while (!loadOperation.isDone)
        {
            string loadingText = "Loading... ";
            loadingText = loadingText + (int)(loadOperation.progress * 100) + "%";
            GameObject.Find("LoadingPercent").GetComponent<Text>().text = loadingText;
            yield return null;
        }

    }

}
