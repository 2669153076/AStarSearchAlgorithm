using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonAutoMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T GetInstance()
    {
        if (instance == null)
        {
            //创建一个空物体，设置对象的名字为脚本名
            GameObject obj = new GameObject(typeof(T).ToString());
            instance = obj.AddComponent<T>();
            DontDestroyOnLoad(obj);
        }
        return instance;
    }


}
