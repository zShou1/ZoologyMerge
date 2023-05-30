using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    protected static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));
                if (instance == null)
                {
                    string name = typeof(T).Name;
                    Debug.LogFormat("Create singleton object: {0}", name);
                    instance = new GameObject(name).AddComponent<T>();
                    if (instance == null)
                    {
                        Debug.LogWarning("Can't find singleton object: " + typeof(T).Name);
                        Debug.LogError("Can't create singleton object: " + typeof(T).Name);
                        return null;
                    }
                }
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        CheckInstance();
    }

    protected bool CheckInstance()
    {
        if (instance == null)
        {
            instance = (T)this;
            return true;
        }

        if (Instance == this)
        {
            return true;
        }

        Destroy(this);
        return false;
    }

    protected void DontDestroyOnLoad()
    {
        DontDestroyOnLoad(gameObject);
    }
}