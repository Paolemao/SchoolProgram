using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Win_UI : MonoBehaviour {

    // Use this for initialization
    public GameObject player;
    Image win;
 
	void Start () {
        win = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {

        if (player.GetComponent<Character>().Win)
        {
            win.GetComponentInChildren<Text>().text = "你赢了";
        }
	}
}
