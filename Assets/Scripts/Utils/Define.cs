using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public enum E_CharName
{
    Null,
    Minju,
    Seolha,
    Yerin
}

public enum E_CharNameKor
{
    홍민주,
    최설하,
    황예린,
}

public abstract class Char : MonoBehaviour
{
    public string Name { get; set; }
    public float MaxHP { get; set; }
    float _nowHP;
    public float NowHP
    {
        get { return _nowHP; }
        set
        {
            _nowHP = value;
            Debug.Log($"현재 HP: {_nowHP}");
        }
    }
    public float ShieldAmount { get; set; }
    public List<Buff> BuffList { get; set; }
}

public abstract class PlayableChar : Char
{
    public int MaxEnergy { get; set; }
    public int NowEnergy { get; set; }
    public List<CardData> Deck { get; set; }

    public PlayableChar() { Deck = new List<CardData>(); }
}


public class Buff
{

}

public enum E_CardType { Attack, Defence, Skill }
public enum E_CardOwner { Minju, Seolha, Yerin }
public enum E_CardColor { Magenta, Cyan, Yellow, Black }
public enum E_CardTier { Normal, Rare }
public enum E_TargetType
{
    None,
    TargetEnemy,
    AllEnemies,
    Self,
    MaxCount
}

public enum E_EffectType
{
    Strength, Crystallization, Blessing, Vulnerability, Weakening, Thorn, Bloodstain, Chain, Encroachment, Blade, BulletMark,
    Injury, Concussion, Despair, MuscleLoss, Scabbard, Interval, Damage, Shield, Heal, Black,  MaxCount
}

[System.Serializable]
public class CardEffectData
{
    public int EffectID;
    public E_TargetType TargetType;
    public E_EffectType CardEffectType;
    public float Amount;
    public string InfoString;

    public CardEffectData(CardEffectData cardEffectData)
    {
        EffectID = cardEffectData.EffectID;
        TargetType = cardEffectData.TargetType;
        CardEffectType = cardEffectData.CardEffectType;
        Amount = cardEffectData.Amount;
        InfoString = cardEffectData.InfoString;
    }

    public CardEffectData() { }
}
[System.Serializable]
public class CardData
{
    public E_CardType CardType;
    public E_CardOwner CardOwner;
    public E_CardColor CardColor;
    public E_CardTier CardTier;
    public int CardCost;
    public string CardName;
    public string CardInfoText;
    public bool NeedTarget;
    public string CardSpriteNameString;
    public List<CardEffectData> CardEffectList;

    public CardData()
    {
        CardEffectList = new List<CardEffectData>();
    }
}

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Inst { get; private set; }
    protected virtual void Awake() => Inst = FindObjectOfType(typeof(T)) as T;
}
