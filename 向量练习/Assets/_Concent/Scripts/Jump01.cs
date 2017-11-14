using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump01 : MonoBehaviour {

    // Use this for initialization
    Rigidbody rigi;
    CapsuleCollider collider;
    public float jumpPower;

    bool isGround;
    bool isJump=false;

    public float groundCheck = 1.5f;
    float height;
	void Start () {
        rigi = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
        height = collider.height;
        //rigi.mass = 10;
	}
	
	// Update is called once per frame

    private void FixedUpdate()
    {
        Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f)+(Vector3.down * groundCheck), Color.red);
        GroundCheck();
        UPdateControl();
    }
    void UPdateControl()
    {

        if (isGround)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rigi.velocity = new Vector3(rigi.velocity.x,0,rigi.velocity.z);
                rigi.AddForce(0, jumpPower, 0);
                isJump = true;

            }
        }
        else if (isJump)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rigi.velocity = new Vector3(rigi.velocity.x, 0, rigi.velocity.z);
                rigi.AddForce(0, jumpPower, 0);
                isJump = false;
            }
        }
    }
    void GroundCheck()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast( transform.position + (Vector3.up * 0.1f),Vector3.down, out hitInfo, groundCheck))
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        }
    }
}
