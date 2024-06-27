using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Inst;
    public List<UnitBase> PlayerUnits;
    public List<Vector2> PlayerPozs;
    public List<UnitBase> EnemyUnits;
    public UnitBase TargetMonster;
    public float PlayerMoveDuration;

    [HideInInspector]public int EnergyAmount;
    [HideInInspector] public int NowEnergy;
    public TMPro.TMP_Text TMP_Energy;


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

    public void UpdateBattleUI()
    {
        TMP_Energy.text = $"{NowEnergy}/{EnergyAmount}";
    }

    public void EndPlayerTurn()
    {
        
    }

    public void FillEnergy()
    {
        NowEnergy = EnergyAmount;
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
                PlayerUnits[i].transform.position = PlayerPozs[i];
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

            moveSequence.Append(targetChar.transform.DOMove(PlayerPozs[0], PlayerMoveDuration));

            int pozIndex = 1;
            for (int i = 0; i < PlayerUnits.Count; i++)
            {
                if (i != targetIndex)
                {
                    moveSequence.Join(PlayerUnits[i].transform.DOMove(PlayerPozs[pozIndex], PlayerMoveDuration));
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
}
