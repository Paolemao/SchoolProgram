using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class WWWSignUp : MonoBehaviour {

    string url;
    public InputField userName;
    public InputField Password;
    public GameObject Inimage;
    public GameObject UpImage;

    //public Button signIn;

    private void Start()
    {
        /*http://192.168.199.118:8080/save/?user=boss&data=heihei*/

        
        url = "http://127.0.0.1:8080/save/?"+ "user="+ userName.text+"&"+ "password=" + MD5(Password.text);
        StartCoroutine(OnClick());
    }
    private void Update()
    {

    }

    IEnumerator OnClick()
    {
        WWW ww1 = new WWW(url);
        yield return ww1;

        if (ww1.error != null)
        {
            Debug.Log(ww1.error);
        }
        else
        {
            UpImage.SetActive(false);
            Inimage.SetActive(true);
        }

    }

    string MD5(string souce)
    {
        MD5 md5 = new MD5CryptoServiceProvider();

        //将输入的密码转化成字节数组
        byte[] bPwd = Encoding.UTF8.GetBytes(souce);

        //计算指定字节数组的哈希值
        byte[] bMD5 = md5.ComputeHash(bPwd);

        //释放加密服务提供类的所有资源
        md5.Clear();

        StringBuilder sbMD5Pwd = new StringBuilder();
        for (int i = 0; i < bMD5.Length; i++)
        {
            //将每个字节数据转化为2位的16进制字符
            sbMD5Pwd.Append(bMD5[i].ToString("x2"));
        }
        return sbMD5Pwd.ToString();
    }

}
