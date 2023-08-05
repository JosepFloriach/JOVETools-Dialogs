using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour
{
    private static T instanceType;

    virtual protected void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    virtual protected void Start()
    {
        if (instanceType == null)
        {
            instanceType = gameObject.GetComponent<T>();
        }
        else
        {
            Destroy(gameObject);
        }
    }  
}
