using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeList
{
    RedNode,
    GreenNode,
    BlueNode,
    YellowNode
}
public class NodeFactory
{
    private GameObject RedNode, GreenNode, BlueNode, YellowNode;

    private static NodeFactory instance;

    public static NodeFactory getInstance()
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
    }

    public GameObject CreateNode(NodeList type)
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
            default:
                //Todo : Make ErrorNode Prefab
                temp = GameObject.Instantiate(RedNode);
                break;
        }
        return temp;
    }
}
