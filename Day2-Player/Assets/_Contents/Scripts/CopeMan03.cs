using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopeMan03 : MonoBehaviour {

    public float speed;
    public float anguleSpeed;

    float turnAmount;
    float forwardAmount;
    Rigidbody rigi;
	// Use this for initialization
	void Start () {

        rigi = GetComponent<Rigidbody>();
        speed = 10;
        anguleSpeed = 360;
	}
	
	// Update is called once per frame
	void Update () {
        UpdateInput();
        UpdateMovement();
	}
    void UpdateInput()
    {
        var h = Input.GetAxis("RightInput");
        var v = Input.GetAxis("ForwardInput");

        var moveInput=h*Vector3.right+v*Vector3.forward;

        if (moveInput.magnitude>1f)
        {
            moveInput=moveInput.normalized;
        }

        moveInput = transform.InverseTransformDirection(moveInput);

        turnAmount = Mathf.Atan2(moveInput.x,moveInput.z);

        forwardAmount = moveInput.z;

    }
    void UpdateMovement()
    {
        transform.Rotate(0,turnAmount*anguleSpeed*Time.deltaTime,0);

        var v = transform.forward * forwardAmount * speed;

        v.y = rigi.velocity.y;
        rigi.velocity = v;
    }
}
