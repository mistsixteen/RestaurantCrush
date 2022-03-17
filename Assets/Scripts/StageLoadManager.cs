using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageLoadManager
{
    //SingleTon
    private static StageLoadManager instance;

    private StageInfo currentStage;

    public static StageLoadManager GetInstance()
    {
        if (instance == null)
        {
            instance = new StageLoadManager();
        }
        return instance;
    }

    public StageLoadManager()
    {
        
    }

    public StageInfo GetStageInfo()
    {
        return currentStage;
    }

    public bool isStageLoaded()
    {
        if(currentStage == null)
        {
            return false;
        }
        if (currentStage.CurrentStageType == StageType.StageTypeNone)
            return false;
        return true;
    }

    public void LoadStage_Infinite()
    {
        currentStage = new StageInfo
        {
            CurrentStageType = StageType.StageTypeInfinte,
            BaseXPos = -4,
            BaseYPos = 3.3f,
            NodeXDistance = 1.2f,
            NodeYDistance = -1.2f,
            BoardXSize = 10,
            BoardYSize = 7,
            MoveLeft = 10,
            Score = 0
        };
    }

}
