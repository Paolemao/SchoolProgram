using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(MyUpdate());
        InvokeRepeating("Test", 1, 1000);
    }

    void Test()
    {
        Debug.Log("Coroutine 1");
    }

    IEnumerator MyUpdate()
    {

        yield return new WaitForSeconds(1);
    
        Debug.Log("Coroutine 1");

    }



}
