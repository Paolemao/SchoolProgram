using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class WayFinding : MonoBehaviour {

    const int width = 30;
    const int height = 30;
    public Transform table;

    //关于地图坐标的二维数组
    int[,] map = new int[height, width];

    public const int START = 8;
    const int END = 9;
    const int WALL = 1;
    const int MOUNT = 3;
    const int RIVER = 4;
    int start_x;
    int start_y;

    int end_x;
    int end_y;

    public GameObject Prefab_wall;
    public GameObject Prefab_start;
    public GameObject prefab_end;
    public GameObject prefab_return;
    public GameObject prefab_River;
    public GameObject prefab_Mount;
    GameObject pathParent;

    public List<Pos> walls_pos = new List<Pos>();

    short[,] step_num;

    public GameObject prefab_path;
    public int[,] Map
    {
        get
        {
            return map;
        }
    }

    public void ReadMapFile()
    {

        string path = Application.dataPath + "//" + "map.txt";

        if (!File.Exists(path))
        {
            //Debug.Log("______________");
            return;
        }
        FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
        StreamReader read = new StreamReader(fs, Encoding.Default);

        string strReadLine = "";
        int y = 0;

        //跳过第一行
        read.ReadLine();
        strReadLine = read.ReadLine();

        while (strReadLine != null && y < height)
        {
            for (int x = 0; x < width && x < strReadLine.Length; x++)
            {
                int t;
                switch (strReadLine[x])
                {
                    case '1':
                        t = 1;
                        break;
                    case '8':
                        t = 8;
                        break;
                    case '9':
                        t = 9;
                        break;
                    case '3':
                        t = 3;
                        break;
                    case '4':
                        t = 4;
                        break;
                    default:
                        t = 0;
                        break;
                }
                Map[y, x] = t;
            }
            y += 1;
            strReadLine = read.ReadLine();
        }
        read.Dispose();//文件流释放
        fs.Close();

    }

    void InitMap0()
    {
        var walls = new GameObject();
        walls.name = "Wall";
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (Map[i, j] == WALL)
                {
                    walls_pos.Add(new Pos(j, i));
                    var go = Instantiate(Prefab_wall, new Vector3(j + table.position.x, 0.5f, i + table.position.z), Quaternion.identity, walls.transform);
                }

                else if (Map[i, j] == END)
                {
                    end_x = j;
                    end_y = i;
                }
                else if (Map[i,j]==START)
                {
                    start_x = j;
                    start_y = i;
                }
            }
        }
    }

    delegate bool Func(Pos cur,int ox,int oy);

    delegate bool AstarFunc(AstarPos cur ,int oG, int ox, int oy);
    int cur_depth = 0;
    IEnumerator  BFS()
    {
        //初始化一个任务队列
        List<Pos> queue = new List<Pos>();

        //初始化一个步数数组
        step_num = new short[Map.GetLength(0), Map.GetLength(1)];
        //将步数均复制为最大数方便比较
        for (int i=0;i< Map.GetLength(0);i++)
        {
            for (int j=0;j< Map.GetLength(1);j++)
            {
                step_num[i, j] = short.MaxValue;
            }
        }

        Pos startPos = new Pos(start_x,start_y);

        //委托函数，用来频繁比较获取寻路点
        Func func = (Pos cur, int ox, int oy) =>
          {
              //if (map[cur.y+oy,cur.x+ox]==END)
              //{
              //    step_num[cur.y + oy, cur.x + ox] = (short)(step_num[cur.y, cur.x] + 1);
              //    Debug.Log("寻路完成");
              //    return true;
              //}

              if (map[cur.y + oy, cur.x + ox] == 0)
              {
                  if (step_num[cur.y + oy, cur.x + ox] > step_num[cur.y, cur.x] + 1)
                  {
                      step_num[cur.y + oy, cur.x + ox] = (short)(step_num[cur.y, cur.x] + 1);
                      queue.Add(new Pos(cur.x + ox, cur.y + oy));
                  }
              }
              return false;
          };
        step_num[startPos.y, startPos.x] = 0;
        queue.Add(startPos);

        while (queue.Count>0)
        {
            Pos cur = queue[0];
            queue.RemoveAt(0);


            //处理上下左右的方格
            //上
            if (cur.y>0)
            {
                if (func(cur,0,-1)) { break;}
            }
            //下
            if (cur.y<height-1)
            {
                if (func(cur,0,1)) { break; }
            }
            //左
            if (cur.x>0)
            {
                if (func(cur,-1,0)) { break; }
            }
            //右
            if (cur.x<width-1)
            {
                if (func(cur,1,0)) { break; }
            }

            if (step_num[cur.y, cur.x] > cur_depth)//每层等待0.1秒
            {
                cur_depth = step_num[cur.y, cur.x];
                RefashPath(step_num);
                yield return new WaitForSeconds(0.1f);

            }

        }

    }
    IEnumerator DFS()
    {
        List<Pos> queue = new List<Pos>();
        //初始化一个步数数组
        step_num = new short[Map.GetLength(0), Map.GetLength(1)];
        //将步数均复制为最大数方便比较
        for (int i = 0; i < Map.GetLength(0); i++)
        {
            for (int j = 0; j < Map.GetLength(1); j++)
            {
                step_num[i, j] = short.MaxValue;
            }
        }
        Pos endPos = new Pos(end_x,end_y);
        Pos startPos = new Pos(start_x,start_y);


        Func func = (Pos cur, int ox, int oy) =>
          {
              if (map[cur.y + oy, cur.x + ox] == END)
              {
                  step_num[cur.y + oy, cur.x + ox] = (short)(step_num[cur.y, cur.x ] + 1);

                  Debug.Log("IFoundYou");
                  return true;
              }
              if (map[cur.y + oy, cur.x + ox] ==0)
              {
                  if (step_num[cur.y + oy, cur.x + ox] > step_num[cur.y, cur.x]+1)
                  {
                      step_num[cur.y + oy, cur.x + ox] = (short)(step_num[cur.y, cur.x] + 1);
                      queue.Add(new Pos(cur.x + ox, cur.y + oy));
                  }
              }
              return false;
          };

        step_num[startPos.y, startPos.x] = 0;
        queue.Add(startPos);


        while (queue.Count>0)
        {
            Debug.Log("queue.Count " + queue.Count);
            int min_dis = 0;
            Pos cur = queue[min_dis];
            float minDistance = Pos.AStarDistance(cur,endPos);
            //Debug.Log("Distance"+minDistance);

            for (int i=0;i<queue.Count;i++)//找到一层中最短的路线（点）
            {
                float d = Pos.AStarDistance(queue[i],endPos);
                //Debug.Log("Distance" + d);
                if (d < minDistance)
                {
                    min_dis = i;
                    cur = queue[i];
                    minDistance = d;          
                }
            }

            queue.RemoveAt(min_dis);

            //int last_count = queue.Count;

            //上
            if (cur.y>0)
            {
                if (func(cur, 0, -1)) { break; };
            }

            //下
            if (cur.y < height-1)
            {
                if (func(cur, 0, 1)) { break; };
            }
            //左
            if (cur.x > 0)
            {
                if (func(cur, -1, 0)) { break; };
            }
            //右
            if (cur.x < width-1)
            {
                if (func(cur, 1, 0)) { break; };
            }
            RefashPath(step_num);
            yield return new WaitForSeconds(0.01F);
        }
        yield return null;
        //yield break;
    }
    IEnumerator DFS02()
    {
        List<Pos> queue = new List<Pos>();
        step_num = new short[height, width];
        for (int i=0;i<height;i++)
        {
            for (int j=0;j<width;j++)
            {
                step_num[i, j] = short.MaxValue;
            }
        }

        Pos startPos = new Pos(start_x,start_y);
        Pos endPos = new Pos(end_x,end_y);

        step_num[startPos.y, startPos.x] = 0;
        queue.Add(startPos);

        Func func = (Pos cur, int ox, int oy) =>
          {
              if (map[cur.y + oy, cur.x + ox] == END)
              {
                  Debug.Log("IFindYou");
                  step_num[cur.y + oy, cur.x + ox] = (short)(step_num[cur.y, cur.x] + 1);
                  return true;
              }
              if (map[cur.y + oy, cur.x + ox] == 0)
              {
                  //if (step_num[cur.y + oy, cur.x + ox]>(short)(step_num[cur.y, cur.x] + 1))//不断优化最短路径
                  if (step_num[cur.y + oy, cur.x + ox] == short.MaxValue)//
                  { 
                      step_num[cur.y + oy, cur.x + ox] = (short)(step_num[cur.y, cur.x] + 1);
                      queue.Add(new Pos(cur.x + ox, cur.y + oy));
                  }
              }
              return false;
          };


        while (queue.Count>0)
        {
            Pos cur = queue[queue.Count - 1];
            queue.RemoveAt(queue.Count - 1);

            //上
            if (cur.y > 0)
            {
                if (func(cur, 0, -1)) { break; };
            }

            //下
            if (cur.y < height - 1)
            {
                if (func(cur, 0, 1)) { break; };
            }
            //左
            if (cur.x > 0)
            {
                if (func(cur, -1, 0)) { break; };
            }
            //右
            if (cur.x < width - 1)
            {
                if (func(cur, 1, 0)) { break; };
            }

            RefashPath(step_num);
            yield return null;

        }

    }
    IEnumerator BFSShowPath()
    {
        Pos p =new Pos(end_x, end_y);
        while (true)
        {
            int cur_step = step_num[p.y, p.x];
            if (cur_step==0)
            {
                break;
            }
            if (p.y > 0 && step_num[p.y - 1, p.x] == cur_step - 1)
            {
                p.y -= 1;
            }
            else if (p.y < step_num.GetLength(0) && step_num[p.y + 1, p.x] == cur_step - 1)
            {
                p.y += 1;
            }
            else if (p.x > 0 && step_num[p.y, p.x - 1] == cur_step - 1)
            {
                p.x -= 1;
            }
            else if (p.x <step_num.GetLength(1) && step_num[p.y, p.x + 1] == cur_step - 1)
            {
                p.x += 1;
            }
            if (!p.Equals(new Pos(start_x,start_y)))
            {
                var go = Instantiate(prefab_return, new Vector3(p.x + table.position.x, 0.5f, p.y + table.position.z), Quaternion.identity, pathParent.transform);
                yield return new WaitForSeconds(0.2f);
            }
        }
        yield return null;
    }
    IEnumerator BFS02()
    {
        //初始化一个任务队列
        List<Pos> queue = new List<Pos>();

        int n = 10;

        //Pos mountPos = new Pos();
        //short mountStep;

        //初始化一个步数数组
        step_num = new short[Map.GetLength(0), Map.GetLength(1)];
        //将步数均复制为最大数方便比较
        for (int i = 0; i < Map.GetLength(0); i++)
        {
            for (int j = 0; j < Map.GetLength(1); j++)
            {
                step_num[i, j] = short.MaxValue;
            }
        }

        Pos startPos = new Pos(start_x, start_y);

        //Mount _mount = (int stepPoint) =>
        //  {
        //      if (stepPoint == MOUNT)
        //      {
        //          return false;
        //      }
        //      else
        //      {
        //          return true;
        //      }
        //  };

        //委托函数，用来频繁比较获取寻路点
        Func func = (Pos cur, int ox, int oy) =>
        {
            //if (map[cur.y+oy,cur.x+ox]==END)
            //{
            //    step_num[cur.y + oy, cur.x + ox] = (short)(step_num[cur.y, cur.x] + 1);
            //    Debug.Log("寻路完成");
            //    return true;
            //}

            if (map[cur.y + oy, cur.x + ox] !=WALL)
            {
                if (step_num[cur.y + oy, cur.x + ox] > step_num[cur.y, cur.x] + 1)
                {
                    if (step_num[cur.y, cur.x]<n)
                    {
                        if (map[cur.y, cur.x] == MOUNT)
                        {
                            step_num[cur.y + oy, cur.x + ox] = (short)(step_num[cur.y, cur.x] + 3);
                            return true;
                        }
                        else if (map[cur.y, cur.x] == RIVER)
                        {
                            step_num[cur.y + oy, cur.x + ox] = (short)(step_num[cur.y, cur.x] + 4);
                            return true;
                        }
                        else
                        {
                            step_num[cur.y + oy, cur.x + ox] = (short)(step_num[cur.y, cur.x] + 1);
                        }
                        queue.Add(new Pos(cur.x + ox, cur.y + oy));
                    }

                }
            }
            return false;
        };
        step_num[startPos.y, startPos.x] = 0;
        queue.Add(startPos);



        while (queue.Count > 0)
        {
            Pos cur = queue[0];
            queue.RemoveAt(0);


            //处理上下左右的方格
            //上
            if (cur.y > 0)
            {
                if (func(cur, 0, -1)) { continue; }
            }
            //下
            if (cur.y < height - 1)
            {
                if (func(cur, 0, 1)) { continue; }
            }
            //左
            if (cur.x > 0)
            {
                if (func(cur, -1, 0)) { continue; }
            }
            //右
            if (cur.x < width - 1)
            {
                if (func(cur, 1, 0)) { continue; }
            }

            if (step_num[cur.y, cur.x] > cur_depth)//每层等待0.1秒
            {
                cur_depth = step_num[cur.y, cur.x];
                RefashPath(step_num);
                yield return new WaitForSeconds(0.1f);

            }

        }

    }
    AstarPos[,] astar_search;

    IEnumerator AStar02()
    {
        //List<AstarPos> openList = new List<AstarPos>();
        //List<AstarPos> closeList = new List<AstarPos>();
        astar_search = new AstarPos[Map.GetLength(0), Map.GetLength(1)];//01
        List<Pos> list = new List<Pos>();//02
        Pos startPos = new Pos(start_x,start_y);//03
        Pos endPos = new Pos(end_x, end_y);//04
        astar_search[startPos.y, startPos.x] = new AstarPos(0,0);//05
        list.Add(startPos);//06

        //每个点的处理函数
        Func func = (Pos cur, int ox, int oy) =>
         {
             var o_score = astar_search[cur.y + oy, cur.x + ox];
             if (o_score!=null&&o_score.closed)
             {
                 return false;
             }
             var cur_score = astar_search[cur.y, cur.x];
             Pos o_pos = new Pos(cur.x+ox,cur.y+oy);
             if (map[cur.y+oy,cur.x+ox]==END)
             {
                 var a = new AstarPos(cur_score.G+1,0);
                 a.parent = cur;
                 astar_search[cur.y + oy, cur.x + ox] = a;
                 Debug.Log("寻路完成");
                 return true;
             }
             if (map[cur.y+oy,cur.x+ox]==0)
             {
                 if (o_score == null)
                 {
                     var a = new AstarPos(cur_score.G + 1, Pos.AStarDistance(o_pos, endPos));
                     a.parent = cur;
                     astar_search[cur.y + oy, cur.x + ox] = a;
                     list.Add(o_pos);
                 }
                 else if (o_score.G>cur_score.G+1)
                 {
                     o_score.G = cur_score.G + 1;
                     o_score.parent = cur;
                     if (!list.Contains(o_pos))
                     {
                         list.Add(o_pos);
                     }
                 }
             }
             return false;
         };
       
        while (list.Count > 0)
        {
            //给自定义类的二维数组排序
            list.Sort((Pos a1,Pos a2)=>
            {
                AstarPos b1 = astar_search[a1.y, a1.x];
                AstarPos b2 = astar_search[a2.y, a2.x];

                return b1.CompareTo(b2);
            });
            Pos cur = list[0];
            list.RemoveAt(0);

            astar_search[cur.y, cur.x].closed = true;

            //处理上下左右的方格
            //上
            if (cur.y > 0)
            {
                if (func(cur, 0, -1)) { break; }
            }
            //下
            if (cur.y < height - 1)
            {
                if (func(cur, 0, 1)) { break; }
            }
            //左
            if (cur.x > 0)
            {
                if (func(cur,  -1, 0)) { break; }
            }
            //右
            if (cur.x < width - 1)
            {
                if (func(cur,  1, 0)) { break; }
            }

            short[,] temp_map = new short[map.GetLength(0), map.GetLength(1)];
            for (int i = 0; i < height; ++i)
            {
                for (int j = 0; j < width; ++j)
                {
                    temp_map[i, j] = short.MaxValue;
                    //if (map_search[i,j] != null && map_search[i,j].closed)
                    if (astar_search[i, j] != null)
                    {
                        temp_map[i, j] = (short)astar_search[i, j].F;
                    }
                }
            }
            RefashPath(temp_map);
            yield return new WaitForSeconds(0.1f);

        }
    }



    void Refesh()
    {
        GameObject[] all_go = GameObject.FindGameObjectsWithTag("Path");
        foreach (var go in all_go)
        {
            Destroy(go);
            //Debug.Log("+++++++++++++++++++++++++++++");
        }
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (map[i, j] == START)
                {
                    //Debug.Log("START " + Prefab_start);
                    var go = Instantiate(Prefab_start, new Vector3(j + table.position.x, 0.5f, i + table.position.z), Quaternion.identity, pathParent.transform);
                    go.tag = "Path";
                }
                if (map[i, j] == END)
                {
                    var go = Instantiate(prefab_end, new Vector3(j + table.position.x, 0.5f, i + table.position.z), Quaternion.identity, pathParent.transform);
                    go.tag = "Path";
                }
                if (map[i, j] == RIVER)
                {
                    var go = Instantiate(prefab_River, new Vector3(j + table.position.x, 0.5f, i + table.position.z), Quaternion.identity, pathParent.transform);
                    go.tag = "Path";
                }
                if (map[i, j] == MOUNT)
                {
                    var go = Instantiate(prefab_Mount, new Vector3(j + table.position.x, 0.5f, i + table.position.z), Quaternion.identity, pathParent.transform);
                    go.tag = "Path";
                }
            }
        }
    }
    void RefashPath(short[,] stepNum)
    {
        Refesh();
        //显示探索过的部分
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (map[i, j] ==0&& stepNum[i,j]!=short.MaxValue)
                {
                    var go = Instantiate(prefab_path, new Vector3(j + table.position.x, 0.5f, i + table.position.z), Quaternion.identity, pathParent.transform);
                    var text_go = go.GetComponentInChildren<TextMesh>();
                    text_go.text = ""+stepNum[i, j];
                    go.tag = "Path";
                }
            }
        }

    }

	// Use this for initialization
	void Start () {
        // mapCreat = GetComponent<MapCreat>();
        pathParent = GameObject.Find("PathParent");
        ReadMapFile();
        InitMap0();
        //StartCoroutine(BFS());
        //StartCoroutine(BFSShowPath());
        //StartCoroutine(DFS());
        //StartCoroutine(DFS02());
        //StartCoroutine(BFS02());
        StartCoroutine(AStar02());
    }
    //private void Update()
    //{
    //    StartCoroutine(BFS());
    //}

}

public class Pos: MonoBehaviour
{
    public int x;
    public int y;

    public Pos()
    {

    }

    public Pos(Pos p)
    {
        x = p.x;
        y = p.y;
    }
    public Pos(int _X,int _y)
    {
        x = _X;
        y = _y;
    }

    public bool Equals(Pos q)
    {
        return x == q.x && y == q.y;
    }
    public static float AStarDistance(Pos cur,Pos end)
    {
        float d1 = Mathf.Abs(cur.x-end.x);
        float d2 = Mathf.Abs(cur.y-end.y);

        return d1 + d2;
    }

}
public class AstarPos
{
    public float G = 0;
    public float H = 0;

    public bool closed = false;

    public Pos parent = null;

    public AstarPos(float g,float h)
    {
        G = g;
        H = h;
        closed = false;
    }

    public float F
    {
        get { return G + H; }
    }

    public int CompareTo(AstarPos a2)
    {
        if(F==a2.F)
        {
            return 0;
        }
        if(F>a2.F)
        {
            return 1;
        }
        return -1;
    }

    public bool Equals(AstarPos a)
    {
        if (a.F==F)
        {
            return true;
        }
        return false;
    }

}



