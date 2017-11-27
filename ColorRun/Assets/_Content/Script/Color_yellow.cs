using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Color_yellow : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
