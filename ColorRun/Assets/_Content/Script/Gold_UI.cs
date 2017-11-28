using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gold_UI : MonoBehaviour {

    // Use this for initialization
    Text text;
    public GameObject player;

	void Start () {
        text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {

        text.text="白币数："+player.GetComponent<Character>().Golds_num;
	}
}
