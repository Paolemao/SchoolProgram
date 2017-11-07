using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character {

    public override void UpdateControl()
    {
        base.UpdateControl();
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");
        bool crouch = Input.GetKey(KeyCode.C);
        
        var move = h * Vector3.right + v * Vector3.forward;
        Addmove(move);
        UPdateCrouch(crouch);
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
