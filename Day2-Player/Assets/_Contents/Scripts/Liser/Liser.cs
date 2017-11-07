using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liser : MonoBehaviour {

    public string tagName = "Player";
    public float range = 50f;

    LineRenderer line;
    Vector3 endPoint;
    
	// Use this for initialization
	void Start () {
        line = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {

        RaycastHit hit;
        endPoint = range * transform.forward;
        Debug.DrawRay(transform.position, transform.forward,Color.black,range);
        if (Physics.Raycast(transform.position,transform.forward,out hit,range))
        {
            endPoint = transform.InverseTransformPoint(hit.point);
            if (hit.collider.tag == tagName)
            {
                GamePlayChacter.Player.Dead();
            }
            line.SetPosition(1, endPoint);
        }
	}
}
