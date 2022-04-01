using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

enum NodeStatus
{
    Idle,
    Moving,
    Disappearing,
}

public enum NodeType
{
    None,
    Red,
    Blue,
    Green,
    Yellow
}

public enum ItemType
{
    None,
    Bomb,
    Horizontal,
    Vertical
}


public class Node : MonoBehaviour
{
    private GameBoard NodeBoardObject;
    private Transform myTransform;

    public float moveLength = 0.5f;
    public int moveFrame = 5;

    private int xPos, yPos;

    private Vector3 mousePoint;
    private bool isClicked;
    private bool isIced;
    public bool isActivated;

    private NodeStatus currentState;

    private Queue<Vector3> moveQueue;

    private Vector3 MoveTarget;
    private Vector3 MoveVector;
    private int MoveFrameLeft;

    public ItemType itemType;

    private NodeType nodeType;
    public NodeType NodeType
    {
        get
        {
            if (isIced == true)
                return NodeType.None;
            else
                return nodeType;
        }
        set
        {
            nodeType = value;
        }
    }

    private bool canMove;
    public bool CanMove
    {
        get
        {
            if (isIced == true)
                return false;
            else
                return canMove;
        }
        set
        {
            canMove = value;
        }
    }

    public Node()
    {
        isClicked = false;
        canMove = true;
        isIced = false;
        xPos = 0;
        yPos = 0;
        MoveFrameLeft = 0;
        moveQueue = new Queue<Vector3>();
        currentState = NodeStatus.Idle;
        nodeType = NodeType.None;
        itemType = ItemType.None;
        isActivated = false;
    }

    public void OnIced()
    {
        this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Square");
        isIced = true;
    }

    public void OnAffected()
    {
        if(itemType == ItemType.None && isIced == true)
        {
            this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Circle");
            isIced = false;
            EffectFactory.GetInstance().MakeIceParticleEffect(transform.position, 5);
        }
    }

    public void OrderMove(Vector3 vTarget)
    {
        if(currentState == NodeStatus.Idle)
        {
            MoveTarget = vTarget;
            MoveVector = (MoveTarget - transform.position) / moveFrame;
            MoveFrameLeft = moveFrame;
            currentState = NodeStatus.Moving;
        }
        else
        {
            moveQueue.Enqueue(vTarget);
        }
    }

    void Start()
    {
        NodeBoardObject = GameObject.Find("NodeBoard").GetComponent<GameBoard>();
    }

    void FixedUpdate()
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
                        MoveFrameLeft = moveFrame;
                    }
                }
                break;
            case NodeStatus.Disappearing:
                transform.position += Vector3.down * 0.05f;
                MoveFrameLeft--;

                if (MoveFrameLeft <= 0)
                    Destroy(this.gameObject);

                break;

        }
    }

    public void OnMouseDown()
    {
        //toDo : NodeBoard에 isClicked Check 요청

        if(CanMove == true && NodeBoardObject.IsClickAble())
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

    public int GetXpos()
    {
        return xPos;
    }
    public int GetYpos()
    {
        return yPos;
    }

    public bool IsIdle()
    {
        if (currentState == NodeStatus.Idle)
            return true;
        else
            return false;
    }

    public void SetDisappear()
    {
        currentState = NodeStatus.Disappearing;
        MoveFrameLeft = 5;
        EffectFactory.GetInstance().MakeStarParticleEffect(transform.position, 5);
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
            NodeBoardObject.MovedNode(xPos, yPos, MoveType.Left);
            isClicked = false;
        }
    }

    public Transform GetTransform()
    { 
        if(myTransform == null)
        {
            myTransform = this.gameObject.GetComponent<Transform>(); 
        }
        return myTransform;
    }

    public bool ActiveOnMove()
    {
        if (itemType != ItemType.None)
        {
            return true;
        }
        return false;
    }
}
