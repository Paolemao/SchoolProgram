using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vision01 : MonoBehaviour {

    public float viewRadius = 8.0f;
    public float viewAngleStep = 30;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        DrawFieldOfView();
    }
    void DrawFieldOfView()
    {
        //获得人物最左边射线的向量，相对正前方，角度-45度
        Vector3 forward_left = Quaternion.Euler(0, -45, 0) * transform.forward * viewRadius;
        for (int i=0;i<=viewAngleStep;i++)
        {
            //每条射线都在forwad_left的基础上向右偏转一点，最后正好偏转90度到视野最右侧
            Vector3 v = Quaternion.Euler(0, 90f / viewAngleStep * i, 0) * forward_left;

            //创建射线
            Ray ray = new Ray(transform.position,v);
            RaycastHit hitInfo = new RaycastHit();
            //射线只与两种层碰撞
            int mask = LayerMask.GetMask("Obstacle","Enemey");
            Physics.Raycast(ray,out hitInfo,viewRadius,mask);

            //获取射线终点 向量加位置为新的位置
            Vector3 pos = transform.position + v;
            if (hitInfo.transform!=null)
            {
                //如果碰撞到什么东西，射线终点就变成碰撞的点
                pos = hitInfo.point;
            }
            //从玩家位置到pos画线段，只会在编辑器里看到
            Debug.DrawLine(transform.position,pos,Color.red);

            if (hitInfo.transform != null && hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemey"))
            {
                OnEnemySpotted(hitInfo.transform.gameObject);
            }
        }
    }

    void OnEnemySpotted(GameObject enemy)
    {
        enemy.GetComponent<Enemy>().spottedFrame = Time.frameCount;
        //Debug.Log(enemy.GetComponent<Enemy>().spottedFrame);
    }
}
