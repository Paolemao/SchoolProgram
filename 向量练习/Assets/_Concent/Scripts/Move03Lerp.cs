using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move03Lerp : MonoBehaviour {

    // Use this for initialization
    public float speed;
    public float angularSpeed;

    Vector3 move;
    Vector3 m_point;
    Quaternion b;
    float count = 0.0001f;
    void Start () {
        m_point = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        UpdateControl();

    }
    void UpdateControl()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 m_position = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(m_position);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray,out hitInfo))
            {
                m_point = hitInfo.point;
                move = m_point - transform.position;
                float angle = Vector3.Angle(move, transform.forward);
                b =Quaternion.Euler(0,angle,0)* transform.rotation;
            }
        }
        transform.position = Vector3.Lerp(transform.position, m_point, 0.05f);
        count += 0.0001f;
        //transform.forward = Vector3.Lerp(transform.forward,move, count);
        transform.forward = b*transform.forward;
    }
}
