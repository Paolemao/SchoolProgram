using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitting : MonoBehaviour {

    public string tag;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == tag)
        {
            GamePlayChacter.Player.Dead();
        }
    }
}
