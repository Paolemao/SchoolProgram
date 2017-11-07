using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firing : MonoBehaviour {

    public float speed;

    public GameObject Bullet;
    public Transform MouthPosition;
    public Transform player;




    // Use this for initialization
    public void Start () {
        StartCoroutine(MakeIns());
	}

    // Update is called once per frame
    /*void Update () {
        Bullet = Instantiate(Bullet,MouthPosition.position,MouthPosition.rotation);
        
        Bullet.GetComponent<Rigidbody>().AddForce(speed, 0,0);
	}*/

     IEnumerator MakeIns()
    {
        yield return new WaitForSeconds(0.1f);
        Bullet = Instantiate(Bullet, MouthPosition.position, MouthPosition.rotation);
        var z = transform.TransformDirection(player.forward);
        Bullet.GetComponent<Rigidbody>().AddForce(speed*3, 0,  0);
        Destroy(Bullet,5);
    }

}
