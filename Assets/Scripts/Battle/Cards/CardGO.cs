using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_CardType { Attack, Defence, Skill }
public enum E_CardOwner { Magenta, Cyan, Yellow }
public enum E_CardColor { Magenta, Cyan, Yellow, Black }
public enum E_CardTier { Normal, Rare }
public class CardGO : MonoBehaviour
{
    public CardData thisCardData;
    public UnitBase CardUser;
    public Sprite[] Sprites_Cost;
    public Sprite[] Sprites_Line;
    public SpriteRenderer SR_Cost;
    public SpriteRenderer SR_Line;
    
    [ContextMenu("�ʱ� ����")]
    public void SetCardSprite()
    {
        int index = (int)thisCardData.CardColor;
        SR_Cost.sprite = Sprites_Cost[index];
        SR_Line.sprite = Sprites_Line[index];
    }

    public void UseCard()
    {
        StartCoroutine(UseCardCor());
    }

    IEnumerator UseCardCor()
    {
        //CardOwner�� �� ������ Ƣ���

        //ī�� ȿ������ ���ʴ�� �ߵ���
        foreach(CardEffectData cardEffectData in thisCardData.CardEffectList)
        {
            //Intervalȿ����� �� �ð���ŭ ���
            if(cardEffectData.TargetType == E_TargetType.None && cardEffectData.CardEffectType == E_CardEffectType.Interval)
            {
                yield return new WaitForSeconds(cardEffectData.Amount);
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
                    //Todo : �� �߰�
                    break;
                case E_CardEffectType.Strength:
                    foreach (UnitBase target in targets)
                    {
                        target.ActiveEffects.Add(new Strength(cardEffectData.Amount));
                    }
                    break;
                case E_CardEffectType.Thorn:
                    foreach (UnitBase target in targets)
                    {
                        target.ActiveEffects.Add(new Thorn(cardEffectData.Amount));
                    }
                    break;
                case E_CardEffectType.Bloodstain:
                    foreach (UnitBase target in targets)
                    {
                        target.ActiveEffects.Add(new Bloodstain(cardEffectData.Amount));
                    }
                    break;
                case E_CardEffectType.Concussion:
                    foreach (UnitBase target in targets)
                    {
                        target.ActiveEffects.Add(new Concussion(cardEffectData.Amount));
                    }
                    break;
                case E_CardEffectType.Chain:
                    foreach (UnitBase target in targets)
                    {
                        target.ActiveEffects.Add(new Chain(cardEffectData.Amount));
                    }
                    break;
                case E_CardEffectType.Heal:
                    foreach (UnitBase target in targets)
                    {
                        target.Heal(cardEffectData.Amount);
                    }
                    break;
                case E_CardEffectType.Black:
                    foreach (UnitBase target in targets)
                    {
                        target.Heal(cardEffectData.Amount);
                    }
                    break;
                case E_CardEffectType.Crystallization:
                    foreach (UnitBase target in targets)
                    {
                        target.ActiveEffects.Add(new Crystallization(cardEffectData.Amount));
                    }
                    break;
                case E_CardEffectType.Blade:
                    foreach (UnitBase target in targets)
                    {
                        target.ActiveEffects.Add(new Blade(cardEffectData.Amount));
                    }
                    break;
                case E_CardEffectType.Blessing:
                    foreach (UnitBase target in targets)
                    {
                        target.ActiveEffects.Add(new Blessing(cardEffectData.Amount));
                    }
                    break;
                case E_CardEffectType.Despair:
                    foreach (UnitBase target in targets)
                    {
                        target.ActiveEffects.Add(new Despair(cardEffectData.Amount));
                    }
                    break;
                case E_CardEffectType.MuscleLoss:
                    foreach (UnitBase target in targets)
                    {
                        target.ActiveEffects.Add(new MuscleLoss(cardEffectData.Amount));
                    }
                    break;
                case E_CardEffectType.BulletMark:
                    foreach (UnitBase target in targets)
                    {
                        target.ActiveEffects.Add(new BulletMark(cardEffectData.Amount));
                    }
                    break;
                case E_CardEffectType.Vulnerability:
                    foreach (UnitBase target in targets)
                    {
                        target.ActiveEffects.Add(new Vulnerability(cardEffectData.Amount));
                    }
                    break;
            }

            //ī�� ��� ���� ó��
        }
    }
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
}
