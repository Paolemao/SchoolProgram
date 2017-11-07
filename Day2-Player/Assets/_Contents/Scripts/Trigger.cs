using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour {

    public UnityEvent EnterEvents;
    public UnityEvent ExitEvents;

    public Animator[] animators;

    public Color color;
    public Renderer[] renderers;
    public string colorName = "_EmissionColor";

    //声音
    public AudioSource[] aduioOpens;

    public bool fire=false;
    public Fire f;

    public string TagName="Player";

    private void OnTriggerEnter(Collider other)
    {
        bool openDoor=false;
        if (other.tag != TagName) { return; }
        else { openDoor = true; }

        foreach (Renderer re in renderers)
        {
            re.material.SetColor(colorName, color);
        }

        foreach (Animator anim in animators)
        {
            anim.SetBool("OpenDoor",openDoor);
            anim.SetBool("OpenRight",openDoor);    
        }
        foreach (AudioSource audio in aduioOpens)
        {
            audio.Play();
        }
        f.Update();
        fire = true;
        EnterEvents.Invoke();

    }
    private void OnTriggerExit(Collider other)
    {
        foreach (Renderer re in renderers)
        {
            re.material.SetColor(colorName, Color.white);
        }
        bool openDoor = false;
        foreach (Animator anim in animators)
        {
            anim.SetBool("OpenDoor", openDoor);
            anim.SetBool("OpenRight", openDoor);
        }
        fire = false;
        ExitEvents.Invoke();
    }


}
