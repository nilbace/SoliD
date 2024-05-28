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
        //CardOwner�� �� ������ Ƣ���

        //ī�� ȿ������ ���ʴ�� �ߵ���
        foreach(CardEffectData cardEffectData in CardData.CardEffectList)
        {
            //Intervalȿ����� �� �ð���ŭ ���
            if(cardEffectData.TargetType == E_TargetType.None && cardEffectData.CardEffectType == E_CardEffectType.Interval)
            {
                //�� �ð���ŭ ���
            }

            //��� Ÿ�ٵ��� �޾� �ͼ�
            var targets = GameManager.Battle.GetProperUnits(CardUser, cardEffectData.TargetType);

            //�� ȿ�� Ÿ�Ը��� �˸��� ȿ���� ��󿡰� ����
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
