using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAStar : MonoBehaviour
{
    //格子左上角初始坐标
    public int beginX = -3;
    public int beginY = 5;
    //格子间距
    public int offsetX = 2;
    public int offsetY = -2;
    //地图宽高
    public int mapW = 5;
    public int mapH = 5;

    private Dictionary<string,GameObject> cubeDir = new Dictionary<string,GameObject>();
    private Vector2 beginPos = Vector2.right * -1;  //开始点 给一个负值
    private List<AStarNode> list;   //最短路径列表

    // Start is called before the first frame update
    void Start()
    {
        AStarMgr.GetInstance().InitMapInfo(mapW, mapH);
        

        for (int i = 0; i < mapW; i++)
        {
            for (int j = 0; j < mapH; j++)
            {
                GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                obj.transform.position = new Vector3(beginX + i * offsetX, beginY + j * offsetY, 0);

                obj.name = i + "_" + j;
                cubeDir.Add(obj.name, obj);

                AStarNode node = AStarMgr.GetInstance().starNodes[i, j];
                if(node.type == E_NodeType.Stop)
                {
                    obj.GetComponent<MeshRenderer>().material.color = Color.red;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit raycastHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out raycastHit, 1000))
            {
                if(beginPos == Vector2.right * -1)
                {
                    //清理上次的路径
                    if (list != null)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            cubeDir[list[i].x + "_" + list[i].y].GetComponent<MeshRenderer>().material.color = Color.white;
                        }
                    }

                    //得到起点
                    string[] strs = raycastHit.collider.gameObject.name.Split('_');
                    beginPos = new Vector2(int.Parse(strs[0]), int.Parse(strs[1]));
                    raycastHit.collider.gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
                }
                else
                {
                    //得到终点
                    string[] strs = raycastHit.collider.gameObject.name.Split('_');
                    Vector2 endPos = new Vector2(int.Parse(strs[0]), int.Parse(strs[1]));
                    //raycastHit.collider.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;

                    list = AStarMgr.GetInstance().FindPath(beginPos, endPos); 
                    cubeDir[(int)beginPos.x + "_" + (int)beginPos.y].GetComponent<MeshRenderer>().material.color = Color.white;
                    if (list != null)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            cubeDir[list[i].x + "_" + list[i].y].GetComponent<MeshRenderer>().material.color = Color.green;
                        }
                    }
                    beginPos = Vector2.right * -1;  //清除开始的点
                }
            }
        }
    }
}
