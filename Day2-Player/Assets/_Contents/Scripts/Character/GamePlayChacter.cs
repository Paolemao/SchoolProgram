using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayChacter : MonoBehaviour {

    static PlayerCharacter player;

    public static PlayerCharacter Player
    {
        get
        {
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();
            }
            return player;
        }

    }
}
