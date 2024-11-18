using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarMgr : BaseManager<AStarMgr>
{
    public int mapW;    //地图宽
    public int mapH;    //地图高

    public AStarNode[,] starNodes;  //地图所有的格子对象容器
    private List<AStarNode> openList;   //开启列表
    private List<AStarNode> closeList;  //关闭列表

    /// <summary>
    /// 初始化地图信息
    /// </summary>
    /// <param name="mapH"></param>
    /// <param name="mapW"></param>
    public void InitMapInfo(int mapW, int mapH)
    {
        this.mapW = mapW;
        this.mapH = mapH;
        starNodes = new AStarNode[mapW, mapH];  //初始化容器
        //根据宽高 创建格子 
        for (int i = 0;i<mapW;i++)
        {
            for(int j = 0; j < mapH; j++)
            {
                AStarNode node = new AStarNode(i,j,Random.Range(0,100)<20?E_NodeType.Stop:E_NodeType.Walk);
                starNodes[i,j] = node;
            }
        }
    }

    /// <summary>
    /// 寻路
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    /// <returns></returns>
    public List<AStarNode> FindPath(Vector2 startPos,Vector2 endPos)
    {
        //如果开始的点或结束的点在地图之外
        if (startPos.x < 0 || startPos.x >= mapW || 
            startPos.y < 0 || startPos.y >= mapH || 
            endPos.x < 0 || endPos.x >= mapW || 
            endPos.y < 0 || endPos.y >= mapH)
        {
            return null;
        }
        AStarNode startNode = starNodes[(int)startPos.x,(int)startPos.y];
        AStarNode endNode  = starNodes[(int)endPos.x,(int)endPos.y];
        //如果开始的点或结束的点的类型是障碍物
        if(startNode.type == E_NodeType.Stop||endNode.type == E_NodeType.Stop)
        {
            return null;
        }

        //清空上一次寻路数据
        //清空关闭和开启列表
        closeList.Clear();
        openList.Clear();
        //开始的点放入关闭列表中
        startNode.Clear();
        closeList.Add(startNode);

        //左上
        FindNearlyNodeToOpenList(startNode.x - 1, startNode.y - 1, 1.4f, startNode, endNode);
        //上
        FindNearlyNodeToOpenList(startNode.x, startNode.y - 1, 1.4f, startNode, endNode);
        //右上
        FindNearlyNodeToOpenList(startNode.x + 1, startNode.y - 1, 1.4f, startNode, endNode);
        //左
        FindNearlyNodeToOpenList(startNode.x - 1, startNode.y, 1.4f, startNode, endNode);
        //右
        FindNearlyNodeToOpenList(startNode.x + 1, startNode.y, 1.4f, startNode, endNode);
        //左下
        FindNearlyNodeToOpenList(startNode.x - 1, startNode.y + 1, 1.4f, startNode, endNode);
        //下
        FindNearlyNodeToOpenList(startNode.x, startNode.y + 1, 1.4f, startNode, endNode);
        //右下
        FindNearlyNodeToOpenList(startNode.x + 1, startNode.y + 1, 1.4f, startNode, endNode);

        openList.Sort(SortOpenList);



        return null;
    }

    /// <summary>
    /// 找到邻近的点放到openlist中
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="g">离起点距离</param>
    /// <param name="father">父对象</param>
    /// <param name="endNode">终点</param>
    private void FindNearlyNodeToOpenList(int x, int y, float g, AStarNode father, AStarNode endNode)
    {
        if (x < 0 || x > mapW || y < 0 || y > mapH)
        {
            return;
        }

        AStarNode node = starNodes[x,y];    //邻近的点

        //如果为空，类型为障碍，在关闭、开启列表中
        if (node == null || node.type == E_NodeType.Stop || closeList.Contains(node) || openList.Contains(node))
        {
            return;
        }

        //计算总消耗 f=g+h
        node.father = father;
        node.g = father.g + g;  //离起点的距离 = 父物体离起点的距离 + 我当前离父物体的距离
        node.h  = Mathf.Abs(endNode.x-node.x)+Mathf.Abs(endNode.y-node.y);
        node.f = node.g + node.h;
        

        openList.Add(node);
    }

    /// <summary>
    /// 列表排序函数
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    private int SortOpenList(AStarNode a, AStarNode b)
    {
        if (a.f > b.f)
            return -1;
        else
            return 1;
    }

}
