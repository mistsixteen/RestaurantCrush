using System.Collections;
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

public class GameBoard : MonoBehaviour
{
    [SerializeField]
    private GameObject redNode, GreenNode, BlueNode, YellowNode;

    public float baseXPos;
    public float baseYPos;
    public float NodeXDistance;
    public float NodeYDistance;
    public int boardXSize;
    public int boardYSize;

    GameObject[,] NodeBoard;

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
    public bool isClickAble()
    {
        return true;
    }

    public void TouchedNode(int xPos, int yPos)
    {
        touchedXpos = xPos;
        touchedYpos = yPos;
        Debug.Log("GameBoard : Touched " + touchedXpos + " " + touchedYpos);
    }

    public void ReleasedNode(int xPos, int yPos, MoveType mType)
    {
        if(touchedXpos == xPos && touchedYpos == yPos)
        {
            Debug.Log("GameBoard : Released " + xPos + " " + yPos + mType);
        }
            
    }

    public void InitializeBoard()
    {
        touchedXpos = -1;
        touchedYpos = -1;
        NodeBoard = new GameObject[boardYSize, boardXSize];
        for (int i = 0; i < boardYSize; i++)
        {
            for (int j = 0; j < boardXSize; j++)
            {
                int random = Random.Range(0, 4);
                switch (random)
                {
                    case 0:
                        NodeBoard[i, j] = Instantiate(redNode);
                        break;
                    case 1:
                        NodeBoard[i, j] = Instantiate(GreenNode);
                        break;
                    case 2:
                        NodeBoard[i, j] = Instantiate(BlueNode);
                        break;
                    case 3:
                        NodeBoard[i, j] = Instantiate(YellowNode);
                        break;
                    default:
                        NodeBoard[i, j] = Instantiate(redNode);
                        break;
                }

                Transform rect = NodeBoard[i, j].GetComponent<Transform>();
                NodeBoard[i, j].GetComponent<Node>().SetPosition(i, j);

                rect.SetParent(this.GetComponent<Transform>());

                rect.position = new Vector3(baseXPos + (float)j * NodeXDistance, baseYPos + (float)i * NodeYDistance, 0.0f);
            }
        }
        currentGameState = GameStatus.Idle;
    }
}
