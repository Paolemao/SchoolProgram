using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public enum MovementMode
{
    Rigidbody,
    NavMeshAgent,
}

public class Character : MonoBehaviour {
    public float speed;
    public float AnguleSpeed;
    float groundCheck=0.2f;

    

    float turnAmount;
    float forwardAmount;
    bool OnCrouch;
    bool IsGround;

    public UnityEvent onDead;
    public UnityEvent OnWin;
    bool isDead = false;

    protected bool crouch=false;

    float capsuleHeight;
    Vector3 capsuleCenter;

    CapsuleCollider capsule;
    Animator anim;
    Rigidbody rigi;

    MovementMode moveMode;

    // Use this for initialization
    protected void Start () {
        anim = GetComponent<Animator>();
        capsule = GetComponent<CapsuleCollider>();
        rigi = GetComponent<Rigidbody>();

        capsuleHeight = capsule.height;
        capsuleCenter = capsule.center;

        var range = GetComponent<NavMeshAgent>();

        if (range && range.enabled)
        {
            moveMode = MovementMode.NavMeshAgent;
        }
        else
        {
            moveMode = MovementMode.Rigidbody;
        }
	}

    // Update is called once per frame
    protected void Update () {
        GroundCheck();
        if (!isDead)
        {
            UpdateControl();
        }
        UPdateMovement();
        UPdateAnimotor();
	}
    public virtual void  UpdateControl()
    {
    }
    protected void UPdateMovement()
    {
        
        transform.Rotate(0,turnAmount*AnguleSpeed*Time.deltaTime,0);

        if (moveMode == MovementMode.NavMeshAgent)
        {
            speed = GetComponent<NavMeshAgent>().speed;
            AnguleSpeed = GetComponent<NavMeshAgent>().angularSpeed;
        
        }
        else if (moveMode==MovementMode.Rigidbody)
        {

            var v = forwardAmount* speed * transform.forward;
            if (OnCrouch)
            {
                v = v * 0.3f;
            }
            v.y = rigi.velocity.y;
            rigi.velocity = v;
        }

    }
    protected void UPdateAnimotor()
    {
        anim.SetFloat("Turn",turnAmount,0.1f,Time.deltaTime);

        if (moveMode == MovementMode.Rigidbody)
        {
            anim.SetFloat("Forward", rigi.velocity.magnitude, 0.1f, Time.deltaTime);
        }
        else if (moveMode==MovementMode.NavMeshAgent)
        {
            var rang = GetComponent<NavMeshAgent>();
            anim.SetFloat("Forward",rang.velocity.magnitude,01f,Time.deltaTime);
        }
        anim.SetBool("IsGround",IsGround);
        anim.SetBool("OnCrouch",OnCrouch);

    }

    public void Addmove(Vector3 move)
    {
        if (move.magnitude>1f) { move = move.normalized; }
        move = transform.InverseTransformDirection(move);

        turnAmount = Mathf.Atan2(move.x,move.z);
        forwardAmount = move.z;
    }

    void GroundCheck()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, groundCheck))
        {

            IsGround = true;
        }
        else
        {
            IsGround = false;
        }
        Debug.DrawRay(transform.position + (Vector3.up * 0.1f), Vector3.down, Color.red,groundCheck);
    }
    protected virtual void UPdateCrouch(bool crouch)
    {
        if (IsGround&&crouch)
        {
            if (OnCrouch) return;
            capsule.height = capsule.height/2f ;
            capsule.center = capsule.center/2f ;
            OnCrouch = true;
        }
        else
        {
            capsule.height = capsuleHeight;
            capsule.center = capsuleCenter;
            OnCrouch = false;
        }
    }

     public void Dead()
    {
        if (isDead) return;
        isDead = true;
        anim.Play("Dead");
        Addmove(Vector3.zero);
        capsule.height = 0.2f;
        capsule.center = new Vector3(0, 0.3f, 0);
        onDead.Invoke();
    }


    public void Win()
    {
        anim.Play("Win");
    }
}
