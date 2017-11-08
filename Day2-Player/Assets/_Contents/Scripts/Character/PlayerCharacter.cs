using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : AllCharacter {

    bool canControl = true;
    protected override void UpdateControl()
    {
        if (!canControl) return;
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");
        bool crouch = Input.GetKey(KeyCode.B);
        var move = h * Vector3.right + v * Vector3.forward;
        Movement(move);
        Crouching(crouch);
        Aiming(Input.GetButton("Fire2"));

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Vector3 dashVelocity = transform.forward * 100;
            rigi.AddForce(dashVelocity,ForceMode.VelocityChange);
        }
        //return move;//
    }
    public void Fard()
    {
        StartCoroutine(Farding());
    }

    IEnumerator Farding()
    {
        yield return new WaitForSeconds(2);
        GameCon.Instance.ReloadScene();
    }

}
