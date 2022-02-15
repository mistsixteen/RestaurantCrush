using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



public class NodeScript : MonoBehaviour
{
    [SerializeField]
    private float moveLength;

    private bool isClicked;
    private Vector3 mousePoint;

    private NodeBoardScript NodeBoardObject;
    
    private Pos CurrentPos;

    public NodeScript()
    {
        isClicked = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        NodeBoardObject = GameObject.Find("NodeBoard").GetComponent<NodeBoardScript>();
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
                isClicked = false;
            }
            else if(mousePoint.y - mousePoint2.y > moveLength)
            {
                Debug.Log("DOWN");
                isClicked = false;
            }
            else if (mousePoint2.x - mousePoint.x > moveLength)
            {
                Debug.Log("R");
                isClicked = false;
            }
            else if (mousePoint.x - mousePoint2.x > moveLength)
            {
                Debug.Log("L");
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
            NodeBoardObject.setTouchedObject(CurrentPos);
            mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        
    }
    
    //IPointerUpHandler
    public void OnMouseUp()
    {
        //toDo : NodeBoard에 isClicked 종료 요청
        isClicked = false;
    }

    void SetPosition(Pos position)
    {
        CurrentPos = position;
    }
}
