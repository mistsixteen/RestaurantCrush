﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum GameStatus
{
    startingGame,
    Idle,
    Moving
}
public enum NodeType
{
    None,
    Red,
    Blue,
    Green,
    Yellow
}
public enum MoveType
{
    Up,
    Down,
    Left,
    Right
}

struct NodeContainer
{
    public GameObject nodeObj;
    public NodeType nodeType;
}

public class GameBoard : MonoBehaviour
{
    //Nodes
    public GameObject redNode, GreenNode, BlueNode, YellowNode;

    //Board Information
    public float baseXPos;
    public float baseYPos;
    public float NodeXDistance;
    public float NodeYDistance;
    public int boardXSize;
    public int boardYSize;

    NodeContainer[,] NodeBoard;

    GameStatus currentGameState;
    int touchedXpos, touchedYpos;

    // Start is called before the first frame update
    void Start()
    {
        currentGameState = GameStatus.startingGame;
        InitializeBoard();
    }

    // Update is called once per frame
    void Update()
    {
        
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

        if(touchedXpos == xPos && touchedYpos == yPos)
        {
            switch(mType)
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
                    if (yPos < boardYSize - 1)
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
                    break;
                case MoveType.Right:
                    if (xPos < boardXSize - 1)
                    {
                        mXPos = xPos + 1;
                    }
                    break;
            }
            
            if(MoveThreeMatchCheck(xPos, yPos, mXPos, mYPos)) //3-matched
            {
                /*
                Vector3 temp1 = GetNodePosition(mXPos, mYPos);
                Vector3 temp2 = GetNodePosition(xPos, yPos);
                NodeBoard[yPos, xPos].nodeObj.GetComponent<Node>().OrderMove(temp1);
                NodeBoard[yPos, xPos].nodeObj.GetComponent<Node>().OrderMove(temp2);
                */
            }
            else //non -> flip
            {
                Vector3 temp1 = GetNodePosition(mXPos, mYPos);
                Vector3 temp2 = GetNodePosition(xPos, yPos);
                NodeBoard[yPos, xPos].nodeObj.GetComponent<Node>().OrderMove(temp1);
                NodeBoard[yPos, xPos].nodeObj.GetComponent<Node>().OrderMove(temp2);
            }

        }
        
    }

    bool MoveThreeMatchCheck(int xPos1, int yPos1, int xPos2, int yPos2)
    {
        NodeType[,] currentBoard = new NodeType[boardYSize, boardXSize];
        for(int i = 0; i < boardYSize; i++)
        {
            for(int j = 0; j < boardXSize; j++)
            {
                currentBoard[i, j] = NodeBoard[i, j].nodeType;
            }
        }
        NodeType temp = currentBoard[yPos1, xPos1];
        currentBoard[yPos1, xPos1] = currentBoard[yPos2, xPos2];
        currentBoard[yPos2, xPos2] = temp;

        for (int i = 0; i < boardYSize; i++)
        {
            for (int j = 0; j < boardXSize; j++)
            {
                if (i < boardYSize - 2 && 
                    currentBoard[i, j] == currentBoard[i + 1, j] &&
                    currentBoard[i + 1, j] == currentBoard[i + 2, j])
                    return true;
                if (j < boardXSize - 2 &&
                    currentBoard[i, j] == currentBoard[i, j + 1] &&
                    currentBoard[i, j + 1] == currentBoard[i, j + 2])
                    return true;
            }
        }


        return false;
    }

    Vector3 GetNodePosition(int xPos, int yPos)
    {
        return new Vector3(baseXPos + (float)xPos * NodeXDistance, baseYPos + (float)yPos * NodeYDistance, 0.0f);
    }

    bool IsInstallAble(int xPos, int yPos, NodeType currentNodeType)
    {
        if (xPos > 1 && NodeBoard[yPos, xPos - 1].nodeType == currentNodeType && NodeBoard[yPos, xPos - 2].nodeType == currentNodeType)
            return false;
        if (yPos > 1 && NodeBoard[yPos - 1, xPos].nodeType == currentNodeType && NodeBoard[yPos - 1, xPos].nodeType == currentNodeType)
            return false;
        return true;
    }

    public void InitializeBoard()
    {
        int random = 0;
        touchedXpos = -1;
        touchedYpos = -1;
        NodeBoard = new NodeContainer[boardYSize, boardXSize];
        for (int i = 0; i < boardYSize; i++)
        {
            for (int j = 0; j < boardXSize; j++)
            {
                while (true)
                {
                    random = Random.Range(1, 5);
                    if (IsInstallAble(j, i, (NodeType)(random)) == true)
                        break;
                }
                
                switch (random)
                {
                    case 1:
                        NodeBoard[i, j].nodeObj = Instantiate(redNode);
                        NodeBoard[i, j].nodeType = NodeType.Red;
                        break;
                    case 2:
                        NodeBoard[i, j].nodeObj = Instantiate(BlueNode);
                        NodeBoard[i, j].nodeType = NodeType.Blue;
                        break;
                    case 3:
                        NodeBoard[i, j].nodeObj = Instantiate(GreenNode);
                        NodeBoard[i, j].nodeType = NodeType.Green;
                        break;
                    case 4:
                        NodeBoard[i, j].nodeObj = Instantiate(YellowNode);
                        NodeBoard[i, j].nodeType = NodeType.Yellow;
                        break;
                    default:
                        NodeBoard[i, j].nodeObj = Instantiate(redNode);
                        NodeBoard[i, j].nodeType = NodeType.None;
                        break;
                }

                Transform rect = NodeBoard[i, j].nodeObj.GetComponent<Transform>();
                NodeBoard[i, j].nodeObj.GetComponent<Node>().SetPosition(j, i);

                rect.SetParent(this.GetComponent<Transform>());

                rect.position = GetNodePosition(j, i);
            }
        }
        currentGameState = GameStatus.Idle;
    }
}
