using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class MapCreat02 : MonoBehaviour {

    const int width= 30;
    const int height = 30;

    int[,] map = new int[height, width];

    const int START = 8;
    const int END = 9;
    const int WALL = 1;

    public GameObject Prefab_wall02;

    public void ReadMapFile()
    {
        string path = Application.dataPath + "//" + "map02.txt";
        if (!File.Exists(path))
        {
            return;
        }

        FileStream fs = new FileStream(path,FileMode.Open,FileAccess.Read);
        StreamReader read = new StreamReader(fs,Encoding.Default);
        int y = 0;
        int t;
        string strReading;
        read.ReadLine();
        strReading = read.ReadLine();
        while (y<height&&strReading!=null)
        {
            for (int x=0;x<width&&x<strReading.Length;x++)
            {
                switch (strReading[x])
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
            strReading = read.ReadLine();
        }
        read.Dispose();//文件流释放
        fs.Close();
    }
    void PrintMap()
    {
        var walls = new GameObject();
        walls.name = "walls";

        for (int i=0;i<height;i++)
        {
            for (int j=0;j<width;j++)
            {
                if (map[i, j] == WALL)
                {
                    var go = Instantiate(Prefab_wall02, new Vector3(j * 1, 0.5f, i * 1), Quaternion.identity, walls.transform);
                }
                
            }
        }
    }
   
	void Start () {

        ReadMapFile();
        PrintMap();

    }
	
}
