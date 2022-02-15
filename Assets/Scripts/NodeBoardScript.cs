using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeBoardScript : MonoBehaviour
{
    [SerializeField]
    public GameObject tempNode;
    public float baseXPos;
    public float baseYPos;

    public float NodeXDistance;
    public float NodeYDistance;


    GameObject[,] NodeBoard;

    // Start is called before the first frame update
    void Start()
    {
        NodeBoard = new GameObject[7, 7];
        for(int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                NodeBoard[i,j] = Instantiate(tempNode);
                Transform rect = NodeBoard[i, j].GetComponent<Transform>();
                rect.position = new Vector3(baseXPos + (float)i * NodeXDistance, baseYPos + (float)j * NodeYDistance, 0.0f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
