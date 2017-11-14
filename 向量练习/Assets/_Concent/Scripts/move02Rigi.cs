using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move02Rigi : MonoBehaviour {

    // Use this for initialization
    Rigidbody rigi;
    public float speed;
    public float AngularSpeed;
    Vector3 mousePoint;
    Vector3 move;
    float angle;
    void Start () {
        rigi = GetComponent<Rigidbody>();
        mousePoint = rigi.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void FixedUpdate()
    {
        UpdateControl();

    }
    void UpdateControl()
    {
        
        if (Input.GetMouseButtonDown(1))
        {
            var mousePosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray,out hitInfo,LayerMask.GetMask("Ground")))
            {
                mousePoint = hitInfo.point;
                mousePoint = new Vector3(mousePoint.x,transform.position.y, mousePoint.z);
            }
        }
        move = mousePoint - transform.position;
        angle = Vector3.Angle(transform.forward, move);
        if (move.sqrMagnitude>0.1f)
        {
            if (angle > 1f)
            {
                TurnControl();
            }
            else
            {
                transform.LookAt(mousePoint);
                move = move.normalized;
                rigi.velocity = move * speed;
            }   
        }
        
    }
    void TurnControl()
    {        
        var minAngle = Mathf.Min(angle,Time.deltaTime*AngularSpeed);
        transform.Rotate(Vector3.Cross(transform.forward,move)*minAngle);
    }
}
