using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 格子类型
/// </summary>
public enum E_NodeType
{
    Walk,   //可以行走的地方
    Stop,   //障碍物
}

/// <summary>
/// A星节点
/// </summary>
public class AStarNode
{
    public int x;   //x坐标
    public int y;   //y坐标
    
    public float f; //寻路消耗
    public float g; //距离起点的距离
    public float h; //距离重点的距离
    public AStarNode father;    //父对象

    public E_NodeType type; //格子类型

    public AStarNode(int x,int y,E_NodeType type)
    {
        this.x = x;
        this.y = y;
        this.type = type;
    }

    /// <summary>
    /// 清空
    /// </summary>
    public void Clear()
    {
        father = null;
        f = 0;
        g = 0;
        h = 0;
    }
}
