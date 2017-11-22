using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class MapCreat : MonoBehaviour {

    public const int width=30;
    public const int height=30;
    public Transform table;

    //关于地图坐标的二维数组
    int[,] map = new int[height, width];

    public const int START = 8;
    const int END = 9;
    const int WALL = 1;

    public int start_x;
    public int start_y;

    public int end_x;
    public int end_y;


    public GameObject Prefab_wall;
    public GameObject Prefab_start;
    public GameObject prefab_end;

    public List<Pos> walls_pos=new List<Pos>();

    public int[,] Map
    {
        get
        {
            return map;
        }
    }

    public void Start()
    {
        ReadMapFile();
        InitMap0();
    }

    void InitMap0()
    {
        var walls = new GameObject();
        walls.name = "Wall";
        for (int i=0; i<height;i++)
        {
            for (int j=0;j<width;j++)
            {
                if (Map[i, j] == WALL)
                {
                    walls_pos.Add(new Pos(j,i));
                    var go = Instantiate(Prefab_wall, new Vector3(j + table.position.x, 0.5f, i + table.position.z), Quaternion.identity, walls.transform);
                }
                else if (Map[i,j]==START)
                {
                    start_x = j;
                    start_y = i;
                    var go = Instantiate(Prefab_start, new Vector3(j + table.position.x, 0.5f, i + table.position.z), Quaternion.identity, walls.transform);
                }
                else if (Map[i, j] == END)
                {
                    end_x = j;
                    end_y = i;
                    var go = Instantiate(prefab_end, new Vector3(j + table.position.x, 0.5f, i + table.position.z), Quaternion.identity, walls.transform);
                }
            }
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
        FileStream fs = new FileStream(path,FileMode.Open,FileAccess.Read);
        StreamReader read = new StreamReader(fs,Encoding.Default);

        string strReadLine = "";
        int y = 0;

        //跳过第一行
        read.ReadLine();
        strReadLine = read.ReadLine();


        while (strReadLine!=null&&y<height)
        {
            for (int x=0;x<width&&x<strReadLine.Length;x++)
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


}
