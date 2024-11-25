using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonBasic : MonoBehaviour
{
    public static SingletonBasic Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
