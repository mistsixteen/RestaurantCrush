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
    private GameObject BombItem;

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

        BombItem = Resources.Load<GameObject>("Prefab/Bomb");
    }

    public Node CreateItemBlock(NodeList nodeType, ItemType itemType)
    {
        GameObject tempGameObject;
        Node NodeComponent = null;

        switch (itemType)
        {
            case ItemType.Bomb:
                tempGameObject = GameObject.Instantiate(BombItem);
                NodeComponent = tempGameObject.GetComponent<Node>();
                NodeComponent.NodeType = NodeType.None;
                NodeComponent.itemType = ItemType.Bomb;
                break;
            case ItemType.Horizontal:
                break;
            case ItemType.Vertical:
                break;
        }


        return NodeComponent;
    }

    public Node CreateNode(NodeList type)
    {
        GameObject tempGameObject;
        Node NodeComponent;
        switch (type)
        {
            case NodeList.RedNode:
                tempGameObject = GameObject.Instantiate(RedNode);
                NodeComponent = tempGameObject.GetComponent<Node>();
                NodeComponent.NodeType = NodeType.Red;
                break;
            case NodeList.GreenNode:
                tempGameObject = GameObject.Instantiate(GreenNode);
                NodeComponent = tempGameObject.GetComponent<Node>();
                NodeComponent.NodeType = NodeType.Green;
                break;
            case NodeList.BlueNode:
                tempGameObject = GameObject.Instantiate(BlueNode);
                NodeComponent = tempGameObject.GetComponent<Node>();
                NodeComponent.NodeType = NodeType.Blue;
                break;
            case NodeList.YellowNode:
                tempGameObject = GameObject.Instantiate(YellowNode);
                NodeComponent = tempGameObject.GetComponent<Node>();
                NodeComponent.NodeType = NodeType.Yellow;
                break;
            case NodeList.XNode:
                tempGameObject = GameObject.Instantiate(XNode);
                NodeComponent = tempGameObject.GetComponent<Node>();
                NodeComponent.NodeType = NodeType.None;
                NodeComponent.CanMove = false;
                break;
            default:
                //Todo : Make ErrorNode Prefab
                tempGameObject = GameObject.Instantiate(XNode);
                NodeComponent = tempGameObject.GetComponent<Node>();
                NodeComponent.NodeType = NodeType.None;
                break;
        }
        return NodeComponent;
    }
}
