using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeList
{
    RedNode,
    GreenNode,
    BlueNode,
    YellowNode,
    XNode
}
public class NodeFactory
{
    private GameObject RedNode, GreenNode, BlueNode, YellowNode, XNode;

    private static NodeFactory instance;

    public static NodeFactory GetInstance()
    {
        if(instance == null)
        {
            instance = new NodeFactory();
        }
        return instance;
    }

    public NodeFactory()
    {
        RedNode = Resources.Load<GameObject>("Prefab/RedNode");
        GreenNode = Resources.Load<GameObject>("Prefab/GreenNode");
        BlueNode = Resources.Load<GameObject>("Prefab/BlueNode");
        YellowNode = Resources.Load<GameObject>("Prefab/YellowNode");
        XNode = Resources.Load<GameObject>("Prefab/XNode");
    }

    public Node CreateNode(NodeList type)
    {
        GameObject temp;
        switch (type)
        {
            case NodeList.RedNode:
                temp = GameObject.Instantiate(RedNode);
                break;
            case NodeList.GreenNode:
                temp = GameObject.Instantiate(GreenNode);
                break;
            case NodeList.BlueNode:
                temp = GameObject.Instantiate(BlueNode);
                break;
            case NodeList.YellowNode:
                temp = GameObject.Instantiate(YellowNode);
                break;
            case NodeList.XNode:
                temp = GameObject.Instantiate(XNode);
                break;
            default:
                //Todo : Make ErrorNode Prefab
                temp = GameObject.Instantiate(XNode);
                break;
        }
        return temp.GetComponent<Node>();
    }
}
