using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour {

    public List<Transform> goldPoint;
    public GameObject gold;
    List<GameObject> golds=new List<GameObject>();

    public List<GameObject> Golds
    {
        get
        {
            return golds;
        }
    }

    public void Start()
    {
        CreatGold();
    }
     void CreatGold()
    {
        foreach (Transform _gold in goldPoint)
        {
            gold = Instantiate(gold, _gold.position, Quaternion.identity);
            golds.Add(gold);
        }

    }
}
