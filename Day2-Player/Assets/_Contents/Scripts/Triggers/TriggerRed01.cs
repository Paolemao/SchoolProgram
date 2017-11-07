using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerRed01 : MonoBehaviour {

    public UnityEvent onEnter;
    public UnityEvent onExit;
    public string Tagname = "Player";
    public AudioSource[] audS;
    public Animator[] anims;

    bool open;


    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("触发");

        if (!other.CompareTag(Tagname))
        {
            return;
        }
        //声音
        foreach (AudioSource audio in audS)
        {
            audio.Play();
        }
        //动画
        foreach (Animator ani in anims)
        {
            open = true;
            ani.SetBool("Open", open);
        }
        onEnter.Invoke();
    }
    private void OnTriggerExit(Collider other)
    {
        onExit.Invoke();
    }
}
