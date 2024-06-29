using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableUnit : UnitBase
{
    protected static float sharedBarrier = 0;

    public override float GetBarrier()
    {
        return sharedBarrier;
    }

    public override void AddBarrier(float barrier)
    {
        sharedBarrier += barrier;
    }
    public override void GetDamage(float amount)
    {
        if (sharedBarrier > 0)
        {
            sharedBarrier -= amount;
            if (sharedBarrier < 0)
            {
                NowHp += sharedBarrier; // ���� �������� HP�� ����
                sharedBarrier = 0;
            }
        }
        else
        {
            NowHp -= amount;
        }

        if (NowHp < 0) NowHp = 0;

        Debug.Log($"Player took damage. Barrier: {sharedBarrier}, HP: {NowHp}");
    }

    [ContextMenu("�� ǥ��")]
    public void ShowBarrier()
    {
        Debug.Log(sharedBarrier);
    }
    private new void Start()
    {
        base.Start();
        BattleManager.Inst.PlayerUnits.Add(this);
        BattleManager.Inst.ArrangePlayerChars();
    }
}
