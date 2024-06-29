using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ���� �� ����
/// </summary>
public class BattleManager : MonoBehaviour
{
    public static BattleManager Inst;
    [HideInInspector]
    public List<UnitBase> PlayerUnits;

    [Tooltip("ĳ���� ������ �Ÿ�")]
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
    //���� ���� ResetDatas�� �־� ���� ����
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
    /// ���ο� ������ ���۵� �� ȣ��
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

                //�� ĥ�ϱ� ���� ���� �۾� �ʿ�
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
    /// ��ü�� ��� Ÿ���� �־ ���(��) ��ȯ
    /// </summary>
    /// <param name="unit">��ü</param>
    /// <param name="targetType">���</param>
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
        // PlayerUnits ����Ʈ���� tag�� ownerName.ToString()�� ���� UnitBase�� ã���ϴ�.
        UnitBase ownerUnit = PlayerUnits.Find(unit => unit.tag == ownerName.ToString());

        // ownerUnit�� null�� �ƴϸ� ������ GetProperUnits �Լ��� ȣ���Ͽ� ����� ��ȯ�մϴ�.
        if (ownerUnit != null)
        {
            return GetProperUnits(ownerUnit, targetType);
        }

        // ���� ownerUnit�� ã�� ���ϸ� �� ����Ʈ�� ��ȯ�մϴ�.
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

            // �ִϸ��̼��� �Ϸ�� �� ����Ʈ�� ������ �ٲߴϴ�.
            moveSequence.OnComplete(() =>
            {
                // targetChar�� ����Ʈ�� ù ��°�� �̵��մϴ�.
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

        // �ش� �̸��� ��������Ʈ�� ã�� ���� ��� null ��ȯ
        return null;
    }
}
