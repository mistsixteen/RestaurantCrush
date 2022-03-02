using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

enum NodeStatus
{
    Idle,
    Moving
}

public class Node : MonoBehaviour
{
    private GameBoard NodeBoardObject;

    public float moveLength = 0.5f;
    public int moveFrame = 30;

    private int xPos, yPos;

    private Vector3 mousePoint;
    private bool isClicked;
    
    private NodeStatus currentState;

    private Queue<Vector3> moveQueue;

    private Vector3 MoveTarget;
    private Vector3 MoveVector;
    private int MoveFrameLeft;

    public Node()
    {
        isClicked = false;
        xPos = 0;
        yPos = 0;
        MoveFrameLeft = 0;
        moveQueue = new Queue<Vector3>();
        currentState = NodeStatus.Idle;
    }

    public void OrderMove(Vector3 vTarget)
    {
        if(currentState == NodeStatus.Idle)
        {
            MoveTarget = vTarget;
            MoveVector = (MoveTarget - transform.position) / moveFrame;
            MoveFrameLeft = moveFrame;
            currentState = NodeStatus.Moving;
            Debug.Log(transform.position);
            Debug.Log(MoveTarget);
            Debug.Log(MoveVector);
        }
        else
        {
            moveQueue.Enqueue(vTarget);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        NodeBoardObject = GameObject.Find("NodeBoard").GetComponent<GameBoard>();
    }
    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case NodeStatus.Idle:
                if (isClicked)
                {
                    CheckMouseMovement();
                }
                break;
            case NodeStatus.Moving:
                transform.position += MoveVector;
                MoveFrameLeft--;
                Debug.Log(MoveFrameLeft);

                if (MoveFrameLeft == 0)
                {
                    transform.position = MoveTarget;
                    if(moveQueue.Count == 0)
                    {
                        currentState = NodeStatus.Idle;
                    }
                    else
                    {
                        MoveTarget = moveQueue.Dequeue();
                        MoveVector = (MoveTarget - transform.position) / moveFrame;
                        MoveFrameLeft = 30;

                    }
                }
                break;

        }
    }

    public void OnMouseDown()
    {
        //toDo : NodeBoard에 isClicked Check 요청
        if(NodeBoardObject.isClickAble())
        {
            isClicked = true;
            NodeBoardObject.TouchedNode(xPos, yPos);
            mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        
    }
    
    public void OnMouseUp()
    {
        //toDo : NodeBoard에 isClicked 종료 요청
        isClicked = false;
    }

    public void SetPosition(int inputXPos, int inputYPos)
    {
        xPos = inputXPos;
        yPos = inputYPos;
    }

    public bool IsIdle()
    {
        if (currentState == NodeStatus.Idle)
            return true;
        else
            return false;
    }
    void CheckMouseMovement()
    {
        Vector3 mousePoint2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePoint2.y - mousePoint.y > moveLength)
        {
            NodeBoardObject.MovedNode(xPos, yPos, MoveType.Up);
            isClicked = false;
        }
        else if (mousePoint.y - mousePoint2.y > moveLength)
        {
            NodeBoardObject.MovedNode(xPos, yPos, MoveType.Down);
            isClicked = false;
        }
        else if (mousePoint2.x - mousePoint.x > moveLength)
        {
            NodeBoardObject.MovedNode(xPos, yPos, MoveType.Right);
            isClicked = false;
        }
        else if (mousePoint.x - mousePoint2.x > moveLength)
        {
            NodeBoardObject.MovedNode(xPos, yPos, MoveType.Up);
            isClicked = false;
        }
    }
}
