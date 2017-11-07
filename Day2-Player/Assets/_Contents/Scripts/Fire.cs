using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {

    public Rigidbody rigi;
    public Trigger tri;

	// Use this for initialization
	void Start () {
        rigi.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	public void Update () {
        if (tri.fire)
        {
            rigi.AddForce(0, 100, 0);
        }
	}
}
