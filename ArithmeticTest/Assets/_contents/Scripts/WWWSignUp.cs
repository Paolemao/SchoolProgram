using System.Collections;
using System.Collections.Generic;
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
        url = "http://127.0.0.1:8080/save/?"+ "user="+ userName.text+"&"+ "password=" + Password.text;
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

}
