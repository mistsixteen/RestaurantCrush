using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pos
{
    public Pos(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; set; }
    public int Y { get; set; }
}

public class NodeBoardScript : MonoBehaviour
{
    [SerializeField]
    public GameObject tempNode;
    public float baseXPos;
    public float baseYPos;
    public float NodeXDistance;
    public float NodeYDistance;

    GameObject[,] NodeBoard;

    Pos touchedObj;

    // Start is called before the first frame update
    void Start()
    {
        touchedObj = new Pos(-1, -1);

        NodeBoard = new GameObject[7, 7];
        for(int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                NodeBoard[i,j] = Instantiate(tempNode);
                Transform rect = NodeBoard[i, j].GetComponent<Transform>();
                rect.SetParent(this.GetComponent<Transform>());
                rect.SendMessage("SetPosition", new Pos(i, j), SendMessageOptions.DontRequireReceiver);
                rect.position = new Vector3(baseXPos + (float)i * NodeXDistance, baseYPos + (float)j * NodeYDistance, 0.0f);
            }
        }
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

    public void setTouchedObject(Pos touched)
    {
        touchedObj = touched;
        Debug.Log("Touched " + touchedObj.X + " " + touchedObj.Y);
    }

    public void releaseTouchedObject()
    {
        Debug.Log("Released " + touchedObj.X + " " + touchedObj.Y);
        touchedObj.X = -1;
        touchedObj.Y = -1;
    }
}
