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


    private void Awake()
    {
        Inst = this;
        PlayerUnits = new List<UnitBase>();
        EnemyUnits = new List<UnitBase>();
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
                foreach(UnitBase _unit in PlayerUnits)
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

                // Debug.Log를 사용하여 리스트 순서가 바뀌었음을 확인합니다.
                Debug.Log("PlayerUnits 리스트 순서가 변경되었습니다.");
            });

            moveSequence.Play();
        }
    }
}
