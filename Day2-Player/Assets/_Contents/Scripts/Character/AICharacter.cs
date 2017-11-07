using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AICharacter : Character {

    public override void UpdateControl()
    {
        base.UpdateControl();
        Addmove(GetComponent<NavMeshAgent>().velocity);
        //return GetComponent<NavMeshAgent>().velocity;
    }
}
