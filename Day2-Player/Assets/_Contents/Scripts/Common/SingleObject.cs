using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleObject<T> : MonoBehaviour where T:Component
{

    protected static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance==null)
            {
                _instance = FindObjectOfType<T>();
                if (_instance==null)
                {
                    GameObject ob= new GameObject();
                    _instance = ob.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    public virtual void Awake()
    {
        DontDestroyOnLoad(gameObject);
        name = "_" + typeof(T).Name;
        if (_instance == null)
        {
            _instance = this as T;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
