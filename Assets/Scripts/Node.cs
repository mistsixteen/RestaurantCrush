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
    [SerializeField]
    private float moveLength;

    private bool isClicked;
    private Vector3 mousePoint;

    private GameBoard NodeBoardObject;

    private int xPos, yPos;
    private readonly NodeStatus currentState;

    public Node()
    {
        isClicked = false;
        moveLength = 0.5f;
        xPos = 0;
        yPos = 0;
        currentState = NodeStatus.Idle;
    }

    // Start is called before the first frame update
    void Start()
    {
        NodeBoardObject = GameObject.Find("NodeBoard").GetComponent<GameBoard>();
    }
    // Update is called once per frame
    void Update()
    {
        if(isClicked)
        {
            Vector3 mousePoint2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if(mousePoint2.y - mousePoint.y > moveLength)
            {
                NodeBoardObject.ReleasedNode(xPos, yPos, MoveType.Up);
                isClicked = false;
            }
            else if(mousePoint.y - mousePoint2.y > moveLength)
            {
                NodeBoardObject.ReleasedNode(xPos, yPos, MoveType.Down);
                isClicked = false;
            }
            else if (mousePoint2.x - mousePoint.x > moveLength)
            {
                NodeBoardObject.ReleasedNode(xPos, yPos, MoveType.Right);
                isClicked = false;
            }
            else if (mousePoint.x - mousePoint2.x > moveLength)
            {
                NodeBoardObject.ReleasedNode(xPos, yPos, MoveType.Up);
                isClicked = false;
            }

        }
    }
    //IPointerDownHandler
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
    
    //IPointerUpHandler
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
}
