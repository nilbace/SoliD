using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData
{
    public List<CardData> UserDeckList = new List<CardData>();
    private int _nowGold;
    public int NowGold { get { return _nowGold; } }

    public void AddGold(int amount)
    {
        _nowGold += amount;
        BaseUI.Inst.UpdateUIs();
    }

    public void Init()
    {
        if (PlayerPrefs.HasKey("UserData"))
        {
            LoadData();
        }
        else
        {
            // 货肺款 蜡历 单捞磐 积己
            UserDeckList = new List<CardData>();
            SaveData();
        }
        BaseUI.Inst.UpdateUIs();
    }

    public void LoadData()
    {
        string jsonData = PlayerPrefs.GetString("UserData");
        UserData loadedData = JsonUtility.FromJson<UserData>(jsonData);
        UserDeckList = loadedData.UserDeckList;
    }

    public void SaveData()
    {
        string jsonData = JsonUtility.ToJson(this);
        PlayerPrefs.SetString("UserData", jsonData);
        PlayerPrefs.Save();
    }
}
