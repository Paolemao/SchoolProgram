using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public Rigidbody rigi;
    public Transform t_player;
    public BoxCollider c_player;
    public Transform end_point;
    public MeshRenderer mesh_player;


    //跳跃
    bool isJump_first;
    bool isJump_second;
    bool isGround;
    bool speedUp;
    bool die=false;

    Vector3 startPoint;

    public float speed;
    public float jumpPower;
    float _groundCheck01=0.5f;
    float _groundCheck02=1f;

    List<GameObject> golds=new List<GameObject>();

    void Start () {
        rigi = GetComponent<Rigidbody>();
        t_player = GetComponent<Transform>();
        startPoint = t_player.position;
        isJump_first = false;
        isJump_second = true;
        isGround = true;
        speedUp = false;
        mesh_player.material.color = Color.yellow;
        
    }

    private void FixedUpdate()
    {
        Move();
        UpdateControl();
        //rigi.velocity = transform.forward.normalized * speed;

    }
    void Update () {

        //Vector3 next = t_player.forward.normalized * speed * Time.deltaTime;
        //t_player.position += next;
        GroundCheck();
        ColorControl();
        //transform.Translate(t_player.forward.normalized * speed * Time.deltaTime);
        StartCoroutine(Restart());
    }
    void UpdateControl()
    {
        if (die) { return; }
        if (isJump_first == false)
        {
            StartCoroutine(Jump());
        }
        if (isJump_first==true&&isJump_second==false)
        {
            StartCoroutine(Jump02());
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Runway")
        {
            isJump_first = false;
        }
        if(collision.gameObject.GetComponent<MeshRenderer>().material.color != mesh_player.material.color)
        {
            die = true;
            //speed = 0;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.GetComponent<MeshRenderer>().material.color != mesh_player.material.color)
        {
            die = true;
            //speed = 0;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Gold")
        {
            other.gameObject.SetActive(false);
            golds.Add(other.gameObject);
        }
    }
    void ColorControl()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (!isGround&&speedUp)
            {
                
                StartCoroutine(SpeedUp());
            }
            if (mesh_player.material.color==Color.red)
            {
                mesh_player.material.color = Color.yellow;
            }
            else if(mesh_player.material.color == Color.yellow)
            {
                mesh_player.material.color = Color.red;
            }
        }
    }
    void GroundCheck()
    {
        RaycastHit hitInfo;
        Debug.DrawLine(t_player.position, t_player.position + (Vector3.down * _groundCheck01), Color.red);

        Ray ray01 = new Ray(t_player.position + (Vector3.up * 0.1f),Vector3.down);

        int mask = LayerMask.GetMask("Runway");

        if (Physics.Raycast(ray01, _groundCheck01, mask))
        {
            isGround = true;
        }
        else
        {
            //Debug.Log("+++++++++++++++++++++++++");
            isGround = false;
        }
        if (Physics.Raycast(t_player.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, _groundCheck02))
        {
            speedUp = true;
        }
        else
        {
            speedUp = false;
        }

    }
    void Move()
    {
        if (!die)
        {
            var temp = rigi.velocity;
            temp.z = (transform.forward.normalized * speed).z;
            rigi.velocity = temp;
        }
        else
        {
            rigi.velocity=new Vector3(rigi.velocity.x, rigi.velocity.y, 0);
        }
    }
    IEnumerator Jump()
    {
        if (Input.GetMouseButton(0))
        {
            rigi.velocity = new Vector3(rigi.velocity.x, 0, rigi.velocity.z);
            rigi.AddForce(0, jumpPower, 0, ForceMode.VelocityChange);
            yield return new WaitForSeconds(0.5f);
            isJump_first = true;
            isJump_second = false;

        }
        else
        {
            yield break;
        }
    }
    IEnumerator Jump02()
    {
        if (Input.GetMouseButton(0))
        {
            rigi.velocity = new Vector3(rigi.velocity.x, 0, rigi.velocity.z);
            rigi.AddForce(0, jumpPower, 0, ForceMode.VelocityChange);
            yield return new WaitForSeconds(0.5f);
            isJump_second = true;
        }
        else
        {
            yield break;
        }
    }
    IEnumerator SpeedUp()
    {
        var oldSpeed = speed;
        speed = speed * 1.7f;
        yield return new WaitForSeconds(0.3f);
        speed = oldSpeed;
    }
    IEnumerator Restart()
    {

        if (die)
        {
            yield return new WaitForSeconds(1.5f);
            die = false;
            t_player.position = startPoint;
            mesh_player.material.color = Color.yellow;
            foreach (GameObject obj in golds)
            {
                Debug.Log("+++++++++++++++++++++++++");
                obj.SetActive(true);
            }
        }
    }
}
