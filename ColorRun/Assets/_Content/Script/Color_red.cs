using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Color_red : MonoBehaviour {

	// Use this for initialization

	void Start () {
        gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
