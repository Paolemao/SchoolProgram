using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move01 : MonoBehaviour {

    // Use this for initialization
    Rigidbody rigi;
    Transform tank;
    List<Vector3> endPoints;
    float speed = 10;
    float angluarSpeed = 200;


    void Start() {
        rigi = GetComponent<Rigidbody>();
        tank = GetComponent<Transform>();
        endPoints= new List<Vector3>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(1))
        {
            UpdateControl();
        }

        if (endPoints.Count>0)
        {
            Vector3 v = endPoints[0] - tank.position;
            var dot = Vector3.Dot(v,tank.right);
            Vector3 next = v.normalized * speed * Time.deltaTime;
            float angle = Vector3.Angle(v, tank.forward);

            if (Vector3.SqrMagnitude(v) > 1f)
            {
                float minAngle = Mathf.Min(angle, angluarSpeed * Time.deltaTime);

                //叉乘
                if (angle > 1f)
                {
                    //transform.Rotate(Vector3.Cross(tank.forward, v.normalized), minAngle);
                    if (dot > 0)
                    {
                        transform.Rotate(new Vector3(0, minAngle, 0));
                    }
                    else
                    {
                        transform.Rotate(new Vector3(0,-minAngle,0));
                    }

                }
                else
                {

                    transform.LookAt(endPoints[0]);
                    tank.position += next;
                }
            }
            else
            {
                endPoints.RemoveAt(0);
            }
           
        }
    }
    void UpdateControl()
    {

            //获取屏幕坐标
            Vector3 mousepostion = Input.mousePosition;
            //定义从屏幕
            Ray ray = Camera.main.ScreenPointToRay(mousepostion);

            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    AddEndPoint(hitInfo.point);
                }
                else
                {
                    ReSetEndPoint(hitInfo.point);
                }
                //transform.LookAt(endPoint);
                //transform.Translate(movePoint * 0.1f);
            }
        
    }
    void AddEndPoint(Vector3 endPoint)
    {
        endPoint.y = transform.position.y;
        endPoints.Add(endPoint);
    }
    void ReSetEndPoint(Vector3 endPoint)
    {
        endPoint.y = transform.position.y;
        endPoints.Clear();
        endPoints.Add(endPoint);
    }
}
