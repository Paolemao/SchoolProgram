using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;


public class AllCharacter : MonoBehaviour {

    public UnityEvent onDead;
    public AudioClip dieSound;
    public Wepon equippedWepon;

    //状态
    protected bool isCrouch;
    protected bool isGround;
    protected bool isDead;
    protected bool isAiming;//这里可以为Attacking

    [Range(0.1f, 100)]
    [SerializeField]
    public float speed = 5;

    [Range(1f, 500)]
    [SerializeField]
    public float angularSpeed = 360;

    [SerializeField]
    protected float groundCheck = 0.2f;

    protected Rigidbody rigi;
    protected Animator anim;
    protected CapsuleCollider capsule;
    protected float turnAmount;
    protected float forwardAmount;
    protected Vector3 veloctity;
    protected Vector3 groundNoraml;
    protected float defaultCapsuleHeight;
    protected Vector3 defaultCapsuleCenter;

    #region Cycle
    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        capsule = GetComponent<CapsuleCollider>();
        rigi = GetComponent<Rigidbody>();
        rigi.drag = 8;
        rigi.mass = 30;

        defaultCapsuleHeight = capsule.height;
        defaultCapsuleCenter = capsule.center;

    }

    protected virtual void Update()
    {
        Debug.Log("+++++++++++++++++++++++++++++++++++++++");
        if (!isDead)
        {
            UpdateControl();
        }
        GroundCheck();
        UPdateMovement();
        UpdateAnimotor();
    }

    protected virtual void UpdateControl()
    {
    }

    protected virtual void UPdateMovement()
    {
        //转向控制
        transform.Rotate(0, turnAmount * angularSpeed * Time.deltaTime, 0);

        //移动控制
        veloctity = forwardAmount * speed * transform.forward;
        if (isCrouch || isAiming)
        {
            veloctity *= 0.3f;
        }
        veloctity.y = rigi.velocity.y;
        rigi.velocity = veloctity;
    }
    protected  void UpdateAnimotor()
    {
        anim.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
        anim.SetFloat("Forward", veloctity.magnitude, 0.01f, Time.deltaTime);
        anim.SetBool("OnCrouch", isCrouch);
        anim.SetBool("IsGround", isGround);
       // anim.SetBool("IsAiming", isAiming);
    }
    #endregion

    #region Private
    void GroundCheck()
    {
        RaycastHit hitInfo;
        Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * groundCheck));
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, groundCheck))
        {
            groundNoraml = hitInfo.normal;
            isGround = true;
        }
        else
        {
            isGround = false;
            groundNoraml = Vector3.up;
        }
    }
    void SetCurrentWeapon(Wepon newWeapon,Wepon LastWenpon)
    {
        //卸载持有的武器
        //安装新的武器
        Wepon localLastWeapon = null;
        if (LastWenpon != null)
        {
            localLastWeapon = LastWenpon;
        }
        else if (newWeapon!=equippedWepon)
        {
            localLastWeapon = equippedWepon;
        }

        if (localLastWeapon)
        {
            localLastWeapon.OnUnEquip();
        }

        equippedWepon = newWeapon;

        if (newWeapon)
        {
            newWeapon.OnEquip();
        }
    }


    #endregion

    #region Public
    public bool Fire(bool continuously)
    {
        if (!isAiming) { return false; }

        if (equippedWepon == null) { return false; }

        bool attackWasSuccessful;

        //单击还是连击
        if (continuously)
        {
            attackWasSuccessful = equippedWepon.AttackContinuouslyHandle();
        }
        else
        {
            attackWasSuccessful = equippedWepon.AttackOnceHandle();
        }

        //触发动画
        if (attackWasSuccessful)
        {
            anim.SetTrigger("Fire");
        }
        return attackWasSuccessful;
    }

    public void EquipWeapon(Wepon weapon)
    {
        if (weapon)
        {
            SetCurrentWeapon(weapon,equippedWepon);
        }
    }

    public void UnEquipWeapon(Wepon weapon)
    {
        if (weapon&&weapon==equippedWepon)
        {
            SetCurrentWeapon(null, weapon);
        }
    }

    public void Movement(Vector3 move)
    {
        if (move.magnitude < 1f) move.Normalize();
        move = transform.InverseTransformDirection(move);
        //将move投影到地板的2D平面上
        move = Vector3.ProjectOnPlane(move,groundNoraml);

        turnAmount = Mathf.Atan2(move.x,move.z);
        forwardAmount = move.z;
    }

    public void Crouching(bool crouch)
    {
        if (isGround && crouch)
        {
            if (isCrouch) return;
            capsule.height = capsule.height / 2f;
            capsule.center = capsule.center / 2f;
            isCrouch = true;
        }
        else
        {
            //限制头顶有遮挡时，必须蹲下
            /*Ray crouchRay = new Ray(rigi.position + Vector3.up * capsule.radius * 0.5f, Vector3.up);
            float crouchRayLength = defaultCapsuleHeight - capsule.radius * 0.5f;
            if (Physics.SphereCast(crouchRay, capsule.radius * 0.5f, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                isCrouch = true;
                return;
            }*/

            isCrouch = false;
            capsule.height = defaultCapsuleHeight;
            capsule.center = defaultCapsuleCenter;
        }
    }

    public void Aiming(bool aiming)
    {
        if (isGround || aiming)
        {
            equippedWepon.gameObject.SetActive(true);
            isAiming = true;
        }
        else
        {
            equippedWepon.gameObject.SetActive(false);
            isAiming = false;
        }
    }

    public virtual void Die()
    {
        if (isDead) return;
        isDead = true;
        Movement(Vector3.zero);
        anim.Play("Die");
        capsule.height = 0.2f;
        capsule.center = new Vector3(0,0.3f,0);

        AudioSource.PlayClipAtPoint(dieSound,transform.position);

       /* var agent = GetComponent<NavMeshAgent>();
        if (agent)
        {
            agent.updatePosition = false;
            agent.updateRotation = false;
        }

        //停止行为树
        var bt = GetComponent<NodeCanvas.BehaviourTrees.BehaviourTreeOwner>();
        if (bt)
        {
            bt.StopBehaviour();
        }*/
        onDead.Invoke();
    }

    #endregion


}

