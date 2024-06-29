using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGO : MonoBehaviour
{
    public CardData thisCardData;
    public Sprite[] Sprites_Cost;
    public Sprite[] Sprites_Line;
    public SpriteRenderer SR_Cost;
    public SpriteRenderer SR_Line;
    public TMPro.TMP_Text TMP_Name;
    public TMPro.TMP_Text TMP_Info;
    public TMPro.TMP_Text TMP_Cost;


    [ContextMenu("�ʱ� ����")]
    public void SetCardSprite()
    {
        int index = (int)thisCardData.CardColor;
        SR_Cost.sprite = Sprites_Cost[index];
        SR_Line.sprite = Sprites_Line[index];
        TMP_Name.text = thisCardData.CardName;
        TMP_Info.text = thisCardData.CardInfoText;
        TMP_Cost.text = thisCardData.CardCost.ToString();
    }

    public void UseCard()
    {
        StartCoroutine(UseCardCor());
    }

    IEnumerator UseCardCor()
    {
        GameManager.Battle.UseEnergy(thisCardData.CardCost);
        GameManager.Battle.MoveCharFront(thisCardData.CardOwner);

        //ī�� ȿ������ ���ʴ�� �ߵ���
        foreach(CardEffectData cardEffectData in thisCardData.CardEffectList)
        {
            //Intervalȿ����� �� �ð���ŭ ���
            if(cardEffectData.TargetType == E_TargetType.None && cardEffectData.CardEffectType == E_EffectType.Interval)
            {
                yield return new WaitForSeconds(cardEffectData.Amount);
            }

            //��� Ÿ�ٵ��� �޾� �ͼ�
            var targets = GameManager.Battle.GetProperUnits(thisCardData.CardOwner, cardEffectData.TargetType);

            //�� ȿ�� Ÿ�Ը��� �˸��� ȿ���� ��󿡰� ����
            switch (cardEffectData.CardEffectType)
            {
                case E_EffectType.Damage:
                    foreach (UnitBase target in targets)
                    {
                        target.GetDamage(cardEffectData.Amount);
                    }
                    break;
                case E_EffectType.Shield:
                    foreach(UnitBase target in targets)
                    {
                        target.AddBarrier(cardEffectData.Amount);
                    }
                    break;
               
                case E_EffectType.Heal:
                    foreach (UnitBase target in targets)
                    {
                        target.Heal(cardEffectData.Amount);
                    }
                    break;

                //�� ĥ�ϱ� ���� ���� �۾� �ʿ�
                case E_EffectType.Black:
                    foreach (UnitBase target in targets)
                    {
                        target.Heal(cardEffectData.Amount);
                    }
                    break;

                default:
                    foreach(UnitBase target in targets)
                    {
                        EffectFactory.GetEffectByType(cardEffectData.CardEffectType, cardEffectData.Amount).ApplyEffect(target);
                        target.EffectUpdateAction?.Invoke();
                    }
                    break;
             
            }

            foreach (UnitBase target in targets)
            {
                VisualEffectManager.Inst.InstantiateEffect(cardEffectData.CardEffectType, target);
            }

            HandManager.Inst.DiscardCardFromHand(gameObject);
        }
    }
}

