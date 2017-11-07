using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TrigerNPC : MonoBehaviour {
    public UnityEvent OnEnter;
    public string tagName;
    public Animator anim;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(tagName))
        {
            return;
        }
        anim.Play("WinPose");
        OnEnter.Invoke();
    }

}
