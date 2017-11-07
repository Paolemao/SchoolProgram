using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerRed : MonoBehaviour {

    public UnityEvent onEnter;
    public UnityEvent onExit;
    public string Tagname="player";
    public Animator[] anims;

    bool open;


    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("触发");

            if (!other.CompareTag(Tagname))
            {
                return;
            }


        foreach (Animator ani in anims)
        {
            open = true;
            ani.SetBool("Open",open);
        }
        onEnter.Invoke();
    }
    private void OnTriggerExit(Collider other)
    {
        onExit.Invoke();
    }
}
