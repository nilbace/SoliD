using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempMonster : UnitBase
{
    private float barrier;
    public int StartHP;
    [Multiline]
    public string IntentDatas;

    private new void Start()
    {
        base.Start();
        BattleManager.Inst.EnemyUnits.Add(this);
        NowHp = MaxHP = StartHP;
    }

    public override float GetBarrier()
    {
        return barrier;
    }

    public override void AddBarrier(float barrier)
    {
        this.barrier += barrier;
    }

    public override void GetDamage(float amount)
    {
        if (barrier > 0)
        {
            barrier -= amount;
            if (barrier < 0)
            {
                NowHp += barrier; // 남은 데미지를 HP에 적용
                barrier = 0;
            }
        }
        else
        {
            NowHp -= amount;
        }

        if (NowHp < 0) NowHp = 0;

        Debug.Log($"Monster took damage. Barrier: {barrier}, HP: {NowHp}");
    }

    public void ThisTurnIntent()
    {

    }
}
