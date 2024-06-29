using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 전투 대상과 턴 관리
/// </summary>
public class BattleManager : MonoBehaviour
{
    public static BattleManager Inst;
    [HideInInspector]
    public List<UnitBase> PlayerUnits;

    [Tooltip("캐릭터 사이의 거리")]
    public float PlayerCharOffset;

    [HideInInspector]
    public List<UnitBase> EnemyUnits;
    [HideInInspector]
    public UnitBase TargetMonster;
    public float PlayerMoveDuration;

    [HideInInspector]public int EnergyAmount;
    [HideInInspector] public int NowEnergy;
    public TMPro.TMP_Text TMP_Energy;

    public List<Sprite> EffectIcons;

    //TODO
    //몬스터 쪽은 ResetDatas에 넣어 수정 예정
    private void Awake()
    {
        Inst = this;
        PlayerUnits = new List<UnitBase>();
        EnemyUnits = new List<UnitBase>();
    }

    private void Start()
    {
        EnergyAmount = 3;
        NowEnergy = EnergyAmount;
        UpdateBattleUI();
    }

    /// <summary>
    /// 새로운 전투가 시작될 때 호출
    /// </summary>
    public void ResetDatas()
    {
        
    }


    public void UpdateBattleUI()
    {
        TMP_Energy.text = $"{NowEnergy}/{EnergyAmount}";
    }

    public void EndPlayerTurn()
    {
        HandManager.Inst.DiscardAllCardsFromHand();
        StartMonsterTurn();
        TurnOver();
        StartPlayerTurn();
    }

    public void StartMonsterTurn()
    {
        foreach(TempMonster monster in EnemyUnits)
        {
            var targets = GetProperUnits(monster, monster.NowIntent.CardEffectData.TargetType);
            switch (monster.NowIntent.CardEffectData.CardEffectType)
            {
                case E_EffectType.Damage:
                    foreach (UnitBase target in targets)
                    {
                        target.GetDamage(monster.NowIntent.CardEffectData.Amount);
                    }
                    break;
                case E_EffectType.Shield:
                    foreach (UnitBase target in targets)
                    {
                        target.AddBarrier(monster.NowIntent.CardEffectData.Amount);
                    }
                    break;

                case E_EffectType.Heal:
                    foreach (UnitBase target in targets)
                    {
                        target.Heal(monster.NowIntent.CardEffectData.Amount);
                    }
                    break;

                //색 칠하기 구현 이후 작업 필요
                case E_EffectType.Black:
                    foreach (UnitBase target in targets)
                    {
                        target.Heal(monster.NowIntent.CardEffectData.Amount);
                    }
                    break;

                default:
                    foreach (UnitBase target in targets)
                    {
                        EffectFactory.GetEffectByType(monster.NowIntent.CardEffectData.CardEffectType, monster.NowIntent.CardEffectData.Amount).ApplyEffect(target);
                        target.EffectUpdateAction?.Invoke();
                    }
                    break;
            }
        }


        foreach (TempMonster monster in EnemyUnits)
        {
            monster.ChooseIntent();
        }

    }

    public void TurnOver()
    {
        for (int i = 0; i < PlayerUnits.Count; i++)
        {
            UnitBase unit = PlayerUnits[i];
            for (int j = 0; j < unit.ActiveEffectList.Count; j++)
            {
                EffectBase effect = unit.ActiveEffectList[j];
                effect.NextTurnStarted(unit);
            }
            unit.EffectUpdateAction?.Invoke();
        }

        for (int i = 0; i < EnemyUnits.Count; i++)
        {
            UnitBase unit = EnemyUnits[i];
            for (int j = 0; j < unit.ActiveEffectList.Count; j++)
            {
                EffectBase effect = unit.ActiveEffectList[j];
                effect.NextTurnStarted(unit);
            }
            unit.EffectUpdateAction?.Invoke();
        }

    }
    public void StartPlayerTurn()
    {
        FillEnergy();
        HandManager.Inst.DrawCards(5);
    }

    public void FillEnergy()
    {
        NowEnergy = EnergyAmount;
        UpdateBattleUI();
    }

    public void UseEnergy(int cost)
    {
        NowEnergy -= cost;
        UpdateBattleUI();
    }

    /// <summary>
    /// 주체와 대상 타입을 넣어서 대상(들) 반환
    /// </summary>
    /// <param name="unit">주체</param>
    /// <param name="targetType">대상</param>
    /// <returns></returns>
    public List<UnitBase> GetProperUnits(UnitBase unit, E_TargetType targetType)
    {
        
        List<UnitBase> tempUnits = new List<UnitBase>();

        if (unit.tag == "Monster" && targetType == E_TargetType.TargetEnemy)
        {
            tempUnits.Add(PlayerUnits[0]); return tempUnits;
        }

        switch (targetType)
        {
            case E_TargetType.TargetEnemy:
                tempUnits.Add(TargetMonster);
                break;
            case E_TargetType.AllEnemies:
                foreach(UnitBase _unit in EnemyUnits)
                {
                    if (unit.tag != _unit.tag) tempUnits.Add(_unit);
                }
                break;
            case E_TargetType.Self:
                tempUnits.Add(unit);
                break;
        }
        return tempUnits;
    }

    public List<UnitBase> GetProperUnits(E_CardOwner ownerName, E_TargetType targetType)
    {
        // PlayerUnits 리스트에서 tag가 ownerName.ToString()과 같은 UnitBase를 찾습니다.
        UnitBase ownerUnit = PlayerUnits.Find(unit => unit.tag == ownerName.ToString());

        // ownerUnit이 null이 아니면 기존의 GetProperUnits 함수를 호출하여 결과를 반환합니다.
        if (ownerUnit != null)
        {
            return GetProperUnits(ownerUnit, targetType);
        }

        // 만약 ownerUnit을 찾지 못하면 빈 리스트를 반환합니다.
        return new List<UnitBase>();
    }

    public void ArrangePlayerChars()
    {
        if(PlayerUnits.Count == 3)
        {
            for (int i = 0; i < 3; i++)
            {
                PlayerUnits[i].transform.localPosition = Vector3.left * i * PlayerCharOffset;
            }
        }
    }
    public void MoveCharFront(E_CardOwner cardOwner)
    {
        // cardOwner와 일치하는 캐릭터를 찾습니다.
        GameObject targetChar = null;
        int targetIndex = -1;
        for (int i = 0; i < PlayerUnits.Count; i++)
        {
            if (PlayerUnits[i].tag == cardOwner.ToString())
            {
                targetChar = PlayerUnits[i].gameObject;
                targetIndex = i;
                break;
            }
        }

        // cardOwner를 가장 앞자리로, 나머지는 뒤로 땡깁니다
        if (targetChar != null && targetIndex != -1)
        {
            // DOTween 시퀀스를 사용하여 애니메이션 순서를 관리합니다.
            Sequence moveSequence = DOTween.Sequence();

            moveSequence.Append(targetChar.transform.DOLocalMoveX(0, PlayerMoveDuration));

            int pozIndex = 1;
            for (int i = 0; i < PlayerUnits.Count; i++)
            {
                if (i != targetIndex)
                {
                    moveSequence.Join(PlayerUnits[i].transform.DOLocalMoveX(-PlayerCharOffset*pozIndex, PlayerMoveDuration));
                    pozIndex++;
                }
            }

            // 애니메이션이 완료된 후 리스트의 순서를 바꿉니다.
            moveSequence.OnComplete(() =>
            {
                // targetChar를 리스트의 첫 번째로 이동합니다.
                var movedUnit = PlayerUnits[targetIndex];
                PlayerUnits.RemoveAt(targetIndex);
                PlayerUnits.Insert(0, movedUnit);
            });

            moveSequence.Play();
        }
    }

    public Sprite GetEffectIcon(E_EffectType effect)
    {
        foreach (var icon in EffectIcons)
        {
            if (icon.name == effect.ToString())
            {
                return icon;
            }
        }

        // 해당 이름의 스프라이트를 찾지 못한 경우 null 반환
        return null;
    }
}
