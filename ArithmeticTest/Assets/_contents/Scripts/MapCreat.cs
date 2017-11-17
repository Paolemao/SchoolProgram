using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class MapCreat : MonoBehaviour {

    const int width=30;
    const int height=30;

    //关于地图坐标的二维数组
    int[,] map = new int[height, width];

    const int START = 8;
    const int END = 9;
    const int WALL = 1;

    public GameObject Prefab_wall;

    private void Start()
    {
        ReadMapFile();
        InitMap0();
    }


    void InitMap0()
    {
        var walls = new GameObject();
        walls.name = "Walls";
        for (int i=0; i<height;i++)
        {
            for (int j=0;j<width;j++)
            {
                if (map[i,j]== WALL)
                {
                    var go = Instantiate(Prefab_wall,new Vector3(j*1,0.5f,i*1),Quaternion.identity,walls.transform);
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
                map[y, x] = t;
            }
            y += 1;
            strReadLine = read.ReadLine();
        }
        read.Dispose();//文件流释放
        fs.Close();
        
    }


}
