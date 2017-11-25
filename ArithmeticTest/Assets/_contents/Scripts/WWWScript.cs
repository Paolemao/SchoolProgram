using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WWWScript : MonoBehaviour {

    string url;
    public InputField userName;
    public InputField Password;
    public GameObject Inimage;

    //UI

    private void Start()
    {
        /*http://192.168.199.118:8080/save/?user=boss&data=heihei*/
        url = "http://127.0.0.1:8080/load/?" + "user=" + userName.text + "&" + "password=" + Password.text;
        StartCoroutine(OnClick());
    }
    private void Update()
    {

    }


    IEnumerator OnClick()
    {

        WWW ww2 = new WWW(url);
        yield return ww2;

        if (ww2.error != null)
        {
            Debug.Log(ww2.error);
        }
        else
        {
            Debug.Log(ww2.text);
            if (ww2.text == "1")
            {
                Inimage.SetActive(false);
            }
            else
            {
                Debug.Log("请重新输入");
            }
        }

    }
}
