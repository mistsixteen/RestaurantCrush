using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StageType
{
    StageTypeNone,
    StageTypeInfinte,
    StageTypeTargetScore,
    StageTypeTargetNode
}

public class StageInfo
{
    //Stage Information
    public float BaseXPos;
    public float BaseYPos;
    public float NodeXDistance;
    public float NodeYDistance;
    public int BoardXSize;
    public int BoardYSize;
    public int MoveLeft;
    public int Score;
    public StageType CurrentStageType;

    public int targetScore;

    public StageInfo()
    {
        CurrentStageType = StageType.StageTypeNone;
    }

    public bool IsGameOver()
    {
        if (MoveLeft <= 0)
            return true;
        return false;
    }
    public bool IsGameCleared()
    {
        if(CurrentStageType == StageType.StageTypeTargetScore)
        {
            if (Score >= targetScore)
                return true;
        }
        return false;
    }
    public void GainScore(int gainedScore)
    {
        Score += gainedScore;
    }
}
