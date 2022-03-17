using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StageType
{
    StageTypeNone,
    StageTypeInfinte,
    StageTypeTarget
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

    public StageInfo()
    {
        CurrentStageType = StageType.StageTypeNone;
    }


}
