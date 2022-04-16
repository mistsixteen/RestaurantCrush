using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum GameStatus
{
    startingGame,
    Idle,
    FlipMoving,
    Moving,
    MatchCheck,
    FallCheck,
    Falling,
    GameEnded
}

public enum MoveType
{
    Up,
    Down,
    Left,
    Right
}


public class GameBoard : MonoBehaviour
{

    private StageInfo currentStage;
    private List<Node> onMoveList;
    private List<Node> itemGenerated;
    private Queue<Node> itemActivated;

    Node[,] NodeBoard;
    NodeType[,] MatchBoard;

    bool[,] isMatched;
    bool[,] isAffected;

    GameStatus currentGameState;

    int touchedXpos, touchedYpos;


    private Coroutine routine_MainLoop;

    public GameBoard()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        currentStage = StageLoadManager.GetInstance().GetStageInfo();
        currentGameState = GameStatus.startingGame;
        onMoveList = new List<Node>();
        InitializeBoard();
        routine_MainLoop = StartCoroutine(MainLoop());
    }

    IEnumerator MainLoop()
    {
        while (currentGameState != GameStatus.GameEnded)
        {
            switch (currentGameState)
            {
                case GameStatus.Idle:
                    break;

                case GameStatus.FlipMoving:
                    onMoveList.RemoveAll(item => item.IsIdle() == true);
                    if (onMoveList.Count == 0)
                    {
                        currentGameState = GameStatus.Idle;
                    }
                    break;

                case GameStatus.Moving:
                    onMoveList.RemoveAll(item => item.IsIdle() == true);
                    if (onMoveList.Count == 0)
                    {
                        currentGameState = GameStatus.MatchCheck;
                    }
                    break;

                case GameStatus.MatchCheck:
                    if (MakeThreeMatchList() == true)
                    {
                        //activateItem
                        while(itemActivated.Count != 0)
                        {
                            Debug.Log("Activate!!!");
                            Node tempNode = itemActivated.Dequeue();
                            int Line = 0;
                            // Activate Item -> Chain Item
                            switch (tempNode.itemType)
                            {
                                case ItemType.Bomb:
                                    int xPos = tempNode.GetXpos();
                                    int yPos = tempNode.GetYpos();

                                    for(int i = -1; i <= 1; i++)
                                    {
                                        for(int j = -1; j <= 1; j++)
                                        {
                                            if (i == 0 && j == 0)
                                                continue;
                                            if (isInGrid(yPos + i, xPos + j))
                                            {
                                                isMatched[yPos + i, xPos + j] = true;
                                                EffectFactory.GetInstance().MakeFireEffect(GetNodePosition(xPos + j, yPos + i));
                                                if (NodeBoard[yPos + i, xPos + j].itemType != ItemType.None && NodeBoard[yPos + i, xPos + j].isActivated == false)
                                                {
                                                    NodeBoard[yPos + i, xPos + j].isActivated = true;
                                                    itemActivated.Enqueue(NodeBoard[yPos + i, xPos + j]);
                                                }
                                            }
                                        }
                                    }
                                    break;

                                case ItemType.Horizontal:
                                    Line = tempNode.GetYpos();
                                    for (int i = 0; i < currentStage.BoardXSize; i++)
                                    {
                                        isMatched[Line, i] = true;
                                        EffectFactory.GetInstance().MakeFireEffect(GetNodePosition(i, Line));
                                        if (NodeBoard[Line, i].itemType != ItemType.None && NodeBoard[Line, i].isActivated == false)
                                        {
                                            NodeBoard[Line, i].isActivated = true;
                                            itemActivated.Enqueue(NodeBoard[Line, i]);
                                        }
                                    }
                                    break;

                                case ItemType.Vertical:
                                    Line = tempNode.GetXpos();
                                    for (int i = 0; i < currentStage.BoardYSize; i++)
                                    {
                                        isMatched[i, Line] = true;
                                        EffectFactory.GetInstance().MakeFireEffect(GetNodePosition(Line, i));
                                        if (NodeBoard[i, Line].itemType != ItemType.None && NodeBoard[i, Line].isActivated == false)
                                        {
                                            NodeBoard[i, Line].isActivated = true;
                                            itemActivated.Enqueue(NodeBoard[i, Line]);
                                        }
                                    }

                                    break;
                            }
                        }

                        //delete Node
                        for (int i = 0; i < currentStage.BoardYSize; i++)
                        {
                            for (int j = 0; j < currentStage.BoardXSize; j++)
                            {
                                if (isMatched[i, j] == true)
                                {
                                    Node temp = NodeBoard[i, j];
                                    temp.SetDisappear();
                                    NodeBoard[i, j] = null;
                                    currentStage.GainScore(1);
                                    setAffection(i, j);
                                }
                            }
                        }
                        //주변 노드를 Affect처리
                        for (int i = 0; i < currentStage.BoardYSize; i++)
                        {
                            for (int j = 0; j < currentStage.BoardXSize; j++)
                            {
                                if (isAffected[i, j] == true && NodeBoard[i, j] != null)
                                {
                                    NodeBoard[i, j].OnAffected();
                                }
                            }
                        }
                        //generateItem
                        foreach(Node newItem in itemGenerated)
                        {
                            if (newItem == null)
                            {
                                continue;
                            }
                            int xPos = newItem.GetXpos();
                            int yPos = newItem.GetYpos();
                            if (NodeBoard[yPos, xPos] == null)
                            {
                                NodeBoard[yPos, xPos] = newItem;
                                NodeBoard[yPos, xPos].gameObject.SetActive(true);
                            }
                            else
                                Destroy(newItem.gameObject);
                        }

                        AudioManager.instance.PlayBreakSound();
                        GameSceneUI.instance.UpdateUI(currentStage);
                        currentGameState = GameStatus.FallCheck;
                    }
                    else
                    { //Game Over Check
                        if (currentStage.IsGameOver())
                        {
                            for (int i = 0; i < currentStage.BoardYSize; i++)
                            {
                                for (int j = 0; j < currentStage.BoardXSize; j++)
                                {
                                    Node temp = NodeBoard[i, j];
                                    if (temp != null)
                                        temp.SetDisappear();
                                    NodeBoard[i, j] = null;
                                }
                            }
                            GameSceneUI.instance.GameOverScreen();
                            currentGameState = GameStatus.GameEnded;
                        }
                        else if (currentStage.IsGameCleared())
                        {
                            for (int i = 0; i < currentStage.BoardYSize; i++)
                            {
                                for (int j = 0; j < currentStage.BoardXSize; j++)
                                {
                                    Node temp = NodeBoard[i, j];
                                    if (temp != null)
                                        temp.SetDisappear();
                                    NodeBoard[i, j] = null;
                                }
                            }
                            GameSceneUI.instance.GameClearScreen(currentStage);
                            currentGameState = GameStatus.GameEnded;
                        }
                        else
                            currentGameState = GameStatus.Idle;
                    }
                    break;

                case GameStatus.FallCheck:
                    MakeFallMoveMent();
                    if (onMoveList.Count == 0)
                    {
                        currentGameState = GameStatus.MatchCheck;
                    }
                    else
                        currentGameState = GameStatus.Falling;
                    break;

                case GameStatus.Falling:
                    onMoveList.RemoveAll(item => item.IsIdle() == true);
                    if (onMoveList.Count == 0)
                    {
                        currentGameState = GameStatus.FallCheck;
                    }
                    break;
                default:
                    break;

            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    //현재 이동가능한 상황인지 확인
    public bool IsClickAble()
    {
        if(currentGameState == GameStatus.Idle)
            return true;
        return false;
    }

    public void TouchedNode(int xPos, int yPos)
    {
        touchedXpos = xPos;
        touchedYpos = yPos;
    }

    public void ReleasedNode(int xPos, int yPos)
    {
        if (touchedXpos == xPos && touchedYpos == yPos)
        {
            touchedXpos = -1;
            touchedYpos = -1;
        }
    }

    public void MovedNode(int xPos, int yPos, MoveType mType)
    {
        int mXPos = xPos;
        int mYPos = yPos;

        if (touchedXpos == xPos && touchedYpos == yPos)
        {
            switch (mType)
            {
                case MoveType.Up:
                    if (yPos > 0)
                    {
                        mYPos = yPos - 1;
                    }
                    else
                        return;
                    break;
                case MoveType.Down:
                    if (yPos < currentStage.BoardYSize - 1)
                    {
                        mYPos = yPos + 1;
                    }
                    else
                        return;
                    break;
                case MoveType.Left:
                    if (xPos > 0)
                    {
                        mXPos = xPos - 1;
                    }
                    else
                        return;
                    break;
                case MoveType.Right:
                    if (xPos < currentStage.BoardXSize - 1)
                    {
                        mXPos = xPos + 1;
                    }
                    else
                        return;
                    break;
            }

            if (NodeBoard[yPos, xPos] == null || NodeBoard[mYPos, mXPos] == null
                || NodeBoard[yPos, xPos].CanMove == false || NodeBoard[mYPos, mXPos].CanMove == false)
                return;

            if (MoveThreeMatchCheck(xPos, yPos, mXPos, mYPos) == true) //3-matched
            {
                Vector3 temp1 = GetNodePosition(mXPos, mYPos);
                Vector3 temp2 = GetNodePosition(xPos, yPos);
                NodeBoard[yPos, xPos].OrderMove(temp1);
                NodeBoard[mYPos, mXPos].OrderMove(temp2);
                onMoveList.Add(NodeBoard[yPos, xPos]);
                onMoveList.Add(NodeBoard[mYPos, mXPos]);
                Node temp = NodeBoard[yPos, xPos];
                NodeBoard[yPos, xPos] = NodeBoard[mYPos, mXPos];
                NodeBoard[mYPos, mXPos] = temp;
                NodeBoard[yPos, xPos].SetPosition(xPos, yPos);
                NodeBoard[mYPos, mXPos].SetPosition(mXPos, mYPos);
                currentGameState = GameStatus.Moving;
                currentStage.MoveLeft--;
                GameSceneUI.instance.UpdateUI(currentStage);
                //AudioManager.instance.PlayMoveSound();
            }
            else //non -> flip
            {
                Vector3 temp1 = GetNodePosition(mXPos, mYPos);
                Vector3 temp2 = GetNodePosition(xPos, yPos);
                NodeBoard[yPos, xPos].OrderMove(temp1);
                NodeBoard[yPos, xPos].OrderMove(temp2);
                NodeBoard[mYPos, mXPos].OrderMove(temp2);
                NodeBoard[mYPos, mXPos].OrderMove(temp1);
                onMoveList.Add(NodeBoard[yPos, xPos]);
                onMoveList.Add(NodeBoard[mYPos, mXPos]);
                currentGameState = GameStatus.FlipMoving;

                //Move sound 적당한걸 못 찾음!!
                //AudioManager.instance.PlayMoveSound();
            }

        }
        
    }

    bool MoveThreeMatchCheck(int xPos1, int yPos1, int xPos2, int yPos2)
    {
        if (NodeBoard[yPos1, xPos1].ActiveOnMove() || NodeBoard[yPos2, xPos2].ActiveOnMove())
        {
            if (NodeBoard[yPos1, xPos1].itemType != ItemType.None)
            {
                NodeBoard[yPos1, xPos1].isActivated = true;
                itemActivated.Enqueue(NodeBoard[yPos1, xPos1]);
            }
            if (NodeBoard[yPos2, xPos2].itemType != ItemType.None)
            {
                NodeBoard[yPos2, xPos2].isActivated = true;
                itemActivated.Enqueue(NodeBoard[yPos2, xPos2]);
            }

            return true;
        }
        NodeType[,] MatchBoard = new NodeType[currentStage.BoardYSize, currentStage.BoardXSize];
        for(int i = 0; i < currentStage.BoardYSize; i++)
        {
            for(int j = 0; j < currentStage.BoardXSize; j++)
            {
                if (NodeBoard[i, j] != null)
                    MatchBoard[i, j] = NodeBoard[i, j].NodeType;
                else
                    MatchBoard[i, j] = NodeType.None;
            }
        }
        NodeType temp = MatchBoard[yPos1, xPos1];
        MatchBoard[yPos1, xPos1] = MatchBoard[yPos2, xPos2];
        MatchBoard[yPos2, xPos2] = temp;

        for (int i = 0; i < currentStage.BoardYSize; i++)
        {
            for (int j = 0; j < currentStage.BoardXSize; j++)
            {
                if (i < currentStage.BoardYSize - 2 && MatchBoard[i, j] != NodeType.None &&
                    MatchBoard[i, j] == MatchBoard[i + 1, j] &&
                    MatchBoard[i + 1, j] == MatchBoard[i + 2, j])
                    return true;
                if (j < currentStage.BoardXSize - 2 && MatchBoard[i, j] != NodeType.None &&
                    MatchBoard[i, j] == MatchBoard[i, j + 1] &&
                    MatchBoard[i, j + 1] == MatchBoard[i, j + 2])
                    return true;
            }
        }
        return false;
    }

    void InitializeMatchBoard()
    {
        //Initialize Match Board
        for (int i = 0; i < currentStage.BoardYSize; i++)
        {
            for (int j = 0; j < currentStage.BoardXSize; j++)
            {
                if (NodeBoard[i, j] != null)
                {
                    MatchBoard[i, j] = NodeBoard[i, j].NodeType;
                }
                else
                    MatchBoard[i, j] = NodeType.None;
                isMatched[i, j] = false;
                isAffected[i, j] = false;
            }
        }

        itemGenerated.Clear();

    }


    bool MakeThreeMatchList()
    {
        bool isMatchMade = false;

        InitializeMatchBoard();

        if(itemActivated.Count > 0)
        {
            isMatchMade = true;
        }

        //1. 가로 체크

        for (int i = 0; i < currentStage.BoardYSize; i++)
        {
            for (int j = 0; j < currentStage.BoardXSize;)
            {
                if (MatchBoard[i, j] == NodeType.None)
                {
                    j += 1;
                    continue;
                }

                NodeType temp = MatchBoard[i, j];
                int matchSize = 1;
                int startingPos = j;

                while(true)
                {
                    j += 1;
                    if (j >= currentStage.BoardXSize)
                    {
                        break;
                    }
                    
                    if (temp == MatchBoard[i, j])
                    {
                        matchSize+= 1;
                    }
                    else
                    {
                        break;
                    }
                }
                if(matchSize >= 3)
                {
                    isMatchMade = true;
                    for(int idx = 0; idx < matchSize; idx++)
                    {
                        isMatched[i, startingPos + idx] = true;
                        if (NodeBoard[i, startingPos + idx].itemType != ItemType.None && NodeBoard[i, startingPos + idx].isActivated == false)
                        {
                            NodeBoard[i, startingPos + idx].isActivated = true;
                            itemActivated.Enqueue(NodeBoard[i, startingPos + idx]);
                        }
                    }
                    //match
                    if(matchSize >= 4)
                    {
                        Debug.Log("GenerateItem - Hor");
                        Node newItemNode = NodeFactory.GetInstance().CreateItemBlock(temp, ItemType.Horizontal);
                        newItemNode.SetPosition(startingPos, i);
                        newItemNode.GetTransform().position = GetNodePosition(startingPos, i);
                        newItemNode.gameObject.SetActive(false);
                        itemGenerated.Add(newItemNode);
                    }
                }

            }
        }

        //2. 세로 체크
        for (int i = 0; i < currentStage.BoardXSize; i++)
        {
            for (int j = 0; j < currentStage.BoardYSize;)
            {
                if (MatchBoard[j, i] == NodeType.None)
                {
                    j += 1;
                    continue;
                }
                NodeType temp = MatchBoard[j, i];
                int matchSize = 1;
                int startingPos = j;

                while (true)
                {
                    j += 1;
                    if (j >= currentStage.BoardYSize)
                    {
                        break;
                    }

                    if (temp == MatchBoard[j, i])
                    {
                        matchSize += 1;
                    }
                    else
                    {
                        break;
                    }
                }
                if (matchSize >= 3)
                {
                    isMatchMade = true;
                    for (int idx = 0; idx < matchSize; idx++)
                    {
                        isMatched[startingPos + idx, i] = true;
                        if (NodeBoard[startingPos + idx, i].itemType != ItemType.None && NodeBoard[startingPos + idx, i].isActivated == false)
                        {
                            NodeBoard[startingPos + idx, i].isActivated = true;
                            itemActivated.Enqueue(NodeBoard[i, startingPos + idx]);
                        }
                    }
                    //match
                    if (matchSize >= 4)
                    {
                        Debug.Log("GenerateItem - Ver");
                        Node newItemNode = NodeFactory.GetInstance().CreateItemBlock(temp, ItemType.Vertical);
                        newItemNode.SetPosition(i, startingPos);
                        newItemNode.GetTransform().position = GetNodePosition(i, startingPos);
                        newItemNode.gameObject.SetActive(false);
                        itemGenerated.Add(newItemNode);
                    }
                }

            }
        }
        return isMatchMade;
    }

    Node CreateRandomNode()
    {
        int random = Random.Range(1, 5);

        Node newNode = null;

        switch (random)
        {
            case 1:
                newNode = NodeFactory.GetInstance().CreateNode(NodeList.RedNode);
                break;
            case 2:
                newNode = NodeFactory.GetInstance().CreateNode(NodeList.BlueNode);
                break;
            case 3:
                newNode = NodeFactory.GetInstance().CreateNode(NodeList.GreenNode);
                break;
            case 4:
                newNode = NodeFactory.GetInstance().CreateNode(NodeList.YellowNode);
                break;
            default:
                Debug.LogError("Unidentified Node!!!");
                break;
        }
        return newNode;
    }

    void MakeFallMoveMent()
    {
        const int CANT_MOVE = -999;
        for (int j = 0; j < currentStage.BoardXSize; j++)
        {
            int preset = -1;

            for (int i = currentStage.BoardYSize - 1; i >= 0; i--)
            {
                if(NodeBoard[i, j] == null)
                {
                    int Getfrom = preset;
                    for(int k = i - 1; k >= 0; k--)
                    {
                        if(NodeBoard[k, j] != null)
                        {
                            if(NodeBoard[k, j].CanMove == false)
                            {
                                Getfrom = CANT_MOVE; // 위에 이동불가능한 블록이 있을 경우, 
                                break;
                            }
                            else
                            {
                                Getfrom = k;
                                break;
                            }
                        }
                    }
                    if (Getfrom >= 0)
                    {
                        NodeBoard[Getfrom, j].OrderMove(GetNodePosition(j, i));
                        NodeBoard[i, j] = NodeBoard[Getfrom, j];
                        NodeBoard[Getfrom, j] = null;
                        NodeBoard[i, j].SetPosition(j, i);
                        onMoveList.Add(NodeBoard[i, j]);
                    }
                    else if (Getfrom == CANT_MOVE)
                    {
                        continue;
                    }
                    else //보드 위에서 떨어짐 : 새로운 블록 생성
                    {
                        NodeBoard[i, j] = CreateRandomNode();

                        Transform rect = NodeBoard[i, j].GetTransform();
                        rect.SetParent(this.GetComponent<Transform>());
                        rect.position = GetNodePosition(j, preset);
                        NodeBoard[i, j].OrderMove(GetNodePosition(j, i));
                        onMoveList.Add(NodeBoard[i, j]);
                        NodeBoard[i, j].SetPosition(j, i);
                        preset--;
                    }
                }
            }
        }
    }

    Vector3 GetNodePosition(int xPos, int yPos)
    {
        return new Vector3(currentStage.BaseXPos + (float)xPos * currentStage.NodeXDistance, currentStage.BaseYPos + (float)yPos * currentStage.NodeYDistance, 0.0f);
    }

    bool IsInstallAble(int xPos, int yPos, NodeType currentNodeType)
    {
        if (currentNodeType == NodeType.None)
            return true;
        if (xPos > 1 && NodeBoard[yPos, xPos - 1].NodeType == currentNodeType && NodeBoard[yPos, xPos - 2].NodeType == currentNodeType)
            return false;
        if (yPos > 1 && NodeBoard[yPos - 1, xPos].NodeType == currentNodeType && NodeBoard[yPos - 1, xPos].NodeType == currentNodeType)
            return false;
        return true;
    }

    public void InitializeBoard()
    {
        int random;

        touchedXpos = -1;
        touchedYpos = -1;

        NodeBoard = new Node[currentStage.BoardYSize, currentStage.BoardXSize];
        isMatched = new bool[currentStage.BoardYSize, currentStage.BoardXSize];
        isAffected = new bool[currentStage.BoardYSize, currentStage.BoardXSize];
        MatchBoard =  new NodeType[currentStage.BoardYSize, currentStage.BoardXSize];
        itemActivated = new Queue<Node>();
        itemGenerated = new List<Node>();

        for (int i = 0; i < currentStage.BoardYSize; i++)
        {
            for (int j = 0; j < currentStage.BoardXSize; j++)
            {

                 while (true)
                 {
                     random = Random.Range(1, 5);
                     if (IsInstallAble(j, i, (NodeType)(random)) == true)
                         break;
                 }
                
                switch (random)
                {
                    /*
                     * TODO : NodeType 세팅 부분을 Prefab에 내재화
                     *        or NodeFactory에서 수행
                     */
                    case 1:
                        NodeBoard[i, j] = NodeFactory.GetInstance().CreateNode(NodeList.RedNode);
                        break;
                    case 2:
                        NodeBoard[i, j] = NodeFactory.GetInstance().CreateNode(NodeList.BlueNode);
                        break;
                    case 3:
                        NodeBoard[i, j] = NodeFactory.GetInstance().CreateNode(NodeList.GreenNode);
                        break;
                    case 4:
                        NodeBoard[i, j] = NodeFactory.GetInstance().CreateNode(NodeList.YellowNode);
                        break;
                    case 5:
                        NodeBoard[i, j] = NodeFactory.GetInstance().CreateNode(NodeList.XNode);
                        break;
                    default:
                        Debug.LogError("Unidentified Node!!!");
                        NodeBoard[i, j].NodeType = NodeType.None;
                        break;
                }

                if(i == currentStage.BoardYSize - 4)
                {
                    NodeBoard[i, j].OnIced();
                }

                Transform rect = NodeBoard[i, j].GetTransform();
                NodeBoard[i, j].SetPosition(j, i);

                rect.SetParent(this.GetComponent<Transform>());

                rect.position = GetNodePosition(j, i);
            }
        }
        currentGameState = GameStatus.Idle;
        GameSceneUI.instance.SetObjectUI(currentStage);
        GameSceneUI.instance.UpdateUI(currentStage);
    }

    void setAffection(int yPos, int xPos)
    {
        if (yPos > 0)
        {
            isAffected[yPos - 1, xPos] = true;
        }
        if (yPos < currentStage.BoardYSize - 1)
        {
            isAffected[yPos + 1, xPos] = true;
        }
        if (xPos > 0)
        {
            isAffected[yPos, xPos - 1] = true;
        }
        if (xPos < currentStage.BoardXSize - 1)
        {
            isAffected[yPos, xPos + 1] = true;
        }
    }
    bool isInGrid(int yPos, int xPos)
    {
        if (yPos < 0 || yPos >= currentStage.BoardYSize)
            return false;
        if (xPos < 0 || xPos >= currentStage.BoardXSize)
            return false;
        return true;
    }

}
