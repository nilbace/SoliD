using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager s_instance;
    public static GameManager Inst { get { Init(); return s_instance; } }

    PoolManager _pool = new PoolManager();
    UserData _userData = new UserData();
    public static PoolManager Pool { get { return Inst._pool; } }
    public static BattleManager Battle { get { return BattleManager.Inst; } }
    public static UserData UserData { get { return Inst._userData; } }

    private void Awake()
    {
        Init();
    }

    static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@GameManager");
            if (go == null)
            {
                go = new GameObject { name = "@GameManager" };
                go.AddComponent<GameManager>();
            }

            s_instance = go.GetComponent<GameManager>();

            s_instance._pool.Init();
            s_instance._userData.Init();
        }

    }

    [ContextMenu("∞ÒµÂ »Æ¿Œ")]
    public void ShowGold()
    {
        Debug.Log(s_instance._userData.NowGold);
    }
}
