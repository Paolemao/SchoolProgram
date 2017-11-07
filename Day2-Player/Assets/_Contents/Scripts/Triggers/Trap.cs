using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour {

    public Animator anim; 
    
    private void OnTriggerEnter(Collider other)
    {
        anim.SetBool("Trap",true);
    }

}
