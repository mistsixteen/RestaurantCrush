using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



public class Node : MonoBehaviour
{
    [SerializeField]
    private float moveLength;

    private bool isClicked;
    private Vector3 mousePoint;

    private GameBoard NodeBoardObject;

    private int xPos, yPos;

    public Node()
    {
        isClicked = false;
        moveLength = 0.5f;
        xPos = 0;
        yPos = 0;
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
                Debug.Log("UP");
                NodeBoardObject.releaseTouchedObject(xPos, yPos);
                isClicked = false;
            }
            else if(mousePoint.y - mousePoint2.y > moveLength)
            {
                Debug.Log("DOWN");
                NodeBoardObject.releaseTouchedObject(xPos, yPos);
                isClicked = false;
            }
            else if (mousePoint2.x - mousePoint.x > moveLength)
            {
                Debug.Log("R");
                NodeBoardObject.releaseTouchedObject(xPos, yPos);
                isClicked = false;
            }
            else if (mousePoint.x - mousePoint2.x > moveLength)
            {
                Debug.Log("L");
                NodeBoardObject.releaseTouchedObject(xPos, yPos);
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
            NodeBoardObject.setTouchedObject(xPos, yPos);
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
}
