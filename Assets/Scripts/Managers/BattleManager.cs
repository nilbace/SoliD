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
    /// ��ü�� ��� Ÿ���� �־ ���(��) ��ȯ
    /// </summary>
    /// <param name="unit">��ü</param>
    /// <param name="targetType">���</param>
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
        // cardOwner�� ��ġ�ϴ� ĳ���͸� ã���ϴ�.
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

        // cardOwner�� ���� ���ڸ���, �������� �ڷ� ����ϴ�
        if (targetChar != null && targetIndex != -1)
        {
            // DOTween �������� ����Ͽ� �ִϸ��̼� ������ �����մϴ�.
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

            // �ִϸ��̼��� �Ϸ�� �� ����Ʈ�� ������ �ٲߴϴ�.
            moveSequence.OnComplete(() =>
            {
                // targetChar�� ����Ʈ�� ù ��°�� �̵��մϴ�.
                var movedUnit = PlayerUnits[targetIndex];
                PlayerUnits.RemoveAt(targetIndex);
                PlayerUnits.Insert(0, movedUnit);

                // Debug.Log�� ����Ͽ� ����Ʈ ������ �ٲ������ Ȯ���մϴ�.
                Debug.Log("PlayerUnits ����Ʈ ������ ����Ǿ����ϴ�.");
            });

            moveSequence.Play();
        }
    }
}
