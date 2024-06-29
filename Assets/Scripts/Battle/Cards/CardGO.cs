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


    [ContextMenu("초기 설정")]
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

        //카드 효과들을 차례대로 발동함
        foreach(CardEffectData cardEffectData in thisCardData.CardEffectList)
        {
            //Interval효과라면 그 시간만큼 대기
            if(cardEffectData.TargetType == E_TargetType.None && cardEffectData.CardEffectType == E_EffectType.Interval)
            {
                yield return new WaitForSeconds(cardEffectData.Amount);
            }

            //대상 타겟들을 받아 와서
            var targets = GameManager.Battle.GetProperUnits(thisCardData.CardOwner, cardEffectData.TargetType);

            //각 효과 타입마다 알맞은 효과를 대상에게 적용
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

                //색 칠하기 구현 이후 작업 필요
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

