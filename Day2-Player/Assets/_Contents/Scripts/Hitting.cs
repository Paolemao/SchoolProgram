using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitting : MonoBehaviour {

    public string tagName;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == tagName)
        {
            GamePlayChacter.Player.Die();
        }
    }
}
