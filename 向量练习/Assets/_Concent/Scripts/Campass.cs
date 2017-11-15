using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campass : MonoBehaviour {

    // Use this for initialization
    public Transform player;
    public Transform northward;
    public float rotateSpeed;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 north_campass = Vector3.Cross(Vector3.up, Vector3.right).normalized;

        Quaternion target = Quaternion.LookRotation(Vector3.forward, player.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation,target,rotateSpeed*Time.deltaTime);
	}
}
