using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//[RequireComponent(typeof(NavMeshAgent))]
public class AICharacter : AllCharacter {

    NavMeshAgent agent;

    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        rigi.constraints = RigidbodyConstraints.None |
        RigidbodyConstraints.FreezePosition |
        RigidbodyConstraints.FreezePositionX |
        RigidbodyConstraints.FreezePositionZ;
    }

    protected override void UpdateControl()
    {
        base.UpdateControl();
        Movement(agent.velocity);
        //return GetComponent<NavMeshAgent>().velocity;
    }

    protected override void UPdateMovement()
    {
        //base.UPdateMovement();
        transform.Rotate(0,turnAmount*angularSpeed*Time.deltaTime,0);

        speed = agent.speed;
        angularSpeed = agent.angularSpeed;
        veloctity = agent.velocity;
    }

    public override void Die()
    {
        base.Die();
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
        }
        onDead.Invoke();
    }
}
