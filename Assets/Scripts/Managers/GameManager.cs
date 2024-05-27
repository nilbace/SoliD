using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager s_instance;
    public static GameManager Inst { get { Init(); return s_instance; } }

    PoolManager _pool = new PoolManager();
    public static PoolManager Pool { get { return Inst._pool; } }

    private void Awake()
    {
        Init();
    }

    static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<GameManager>();
            }

            s_instance = go.GetComponent<GameManager>();

            s_instance._pool.Init();
        }

    }
}
