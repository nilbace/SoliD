using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGO : MonoBehaviour
{
    public CardData CardData;
    public UnitBase CardUser;
    
    public void SetCardSprite()
    {

    }

    public void UseCard()
    {
        //CardOwner가 맨 앞으로 튀어나옴

        //카드 효과들을 차례대로 발동함
        foreach(CardEffectData cardEffectData in CardData.CardEffectList)
        {
            //Interval효과라면 그 시간만큼 대기
            if(cardEffectData.TargetType == E_TargetType.None && cardEffectData.CardEffectType == E_CardEffectType.Interval)
            {
                //그 시간만큼 대기
            }

            //대상 타겟들을 받아 와서
            var targets = GameManager.Battle.GetProperUnits(CardUser, cardEffectData.TargetType);

            //각 효과 타입마다 알맞은 효과를 대상에게 적용
            switch (cardEffectData.CardEffectType)
            {
                case E_CardEffectType.Damage:
                    foreach(UnitBase target in targets)
                    {
                        target.GetDamage(cardEffectData.Amount);
                    }
                    break;
                case E_CardEffectType.Encroachment:
                    foreach (UnitBase target in targets)
                    {
                        target.ActiveEffects.Add(new Encroachment(cardEffectData.Amount));
                    }
                    break;
                case E_CardEffectType.Shield:
                    break;
                case E_CardEffectType.Strength:
                    break;
                case E_CardEffectType.Thorn:
                    break;
                case E_CardEffectType.Bloodstain:
                    break;
                case E_CardEffectType.Concussion:
                    break;
                case E_CardEffectType.Chain:
                    break;
                case E_CardEffectType.Heal:
                    break;
                case E_CardEffectType.Black:
                    break;
                case E_CardEffectType.Crystallization:
                    break;
                case E_CardEffectType.Blade:
                    break;
                case E_CardEffectType.Blessing:
                    break;
                case E_CardEffectType.Despair:
                    break;
                case E_CardEffectType.MuscleLoss:
                    break;
                case E_CardEffectType.BulletMark:
                    break;
                case E_CardEffectType.Vulnerability:
                    break;
            }

        }
    }
}

public enum E_CardType { Attack, Defence, Skill}
public enum E_CardOwner { Magenta, Cyan, Yellow}
public enum E_CardColor { Magenta, Cyan, Yellow, Black}
public enum E_CardTier { Normal, Rare}

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
}
