using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerGreen : MonoBehaviour {

    public UnityEvent onEnter;
    public UnityEvent onExit;
    public string Tagname = "Player";
    public AudioSource[] audS;
    public Animator[] anims;

    private GameObject Camera0;
    private GameObject Camera1;
    private GameObject Camera2;
        private GameObject Camera3;

    bool open;
    private void Start()
    {
        Camera0 = GameObject.Find("Main Camera");
        Camera1 = GameObject.Find("PlayerCamera");
        Camera2 = GameObject.Find("PlayerCamera2");
    }


    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("触发");

        if (!other.CompareTag(Tagname))
        {
            return;
        }
        //动画
        foreach (Animator ani in anims)
        {
            open = true;
            ani.SetBool("Open", open);
        }
        Camera1.SetActive(false);
        Camera2.SetActive(true);
        onEnter.Invoke();
    }
    private void OnTriggerExit(Collider other)
    {
        onExit.Invoke();
    }
}
