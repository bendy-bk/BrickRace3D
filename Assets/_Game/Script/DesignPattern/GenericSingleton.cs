using UnityEngine;
using System.Collections;
using Unity.VisualScripting;


public class GenericSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    // để đảm bảo thread-safe (dù Unity hiếm khi cần).  an toàn luồng
    private static object _lock = new object();
    //lưu trữ instance duy nhất của singleton.s
    private static T instance;
    public static T Instance
    {
        get
        {
            //tránh tạo lại Singleton khi game đang thoát.
            if (applicationIsQuitting)
            {
                return null;
            }
            lock (_lock)
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();
                    if (instance == null)
                    {

                        GameObject go = new GameObject();
                        instance = go.AddComponent<T>();
                        go.name = "Singleton";
                        
                        DontDestroyOnLoad(go);//để không bị phá hủy khi load scene.
                    }
                }
                else if (instance != FindObjectOfType<T>())
                {
                    Destroy(FindObjectOfType<T>());
                }
                return instance;
            }
        }
    }
    private static bool applicationIsQuitting = false;
    //Khi singleton bị phá hủy, đặt cờ applicationIsQuitting thành true để ngăn tạo lại khi thoát game.
    public void OnDestroy()
    {
        Debug.Log("Gets destroyed");
        applicationIsQuitting = true;
    }
}