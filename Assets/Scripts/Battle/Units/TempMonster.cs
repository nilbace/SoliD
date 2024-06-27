using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempMonster : UnitBase
{
    public int StartHP;
    private new void Start()
    {
        base.Start();
        BattleManager.Inst.EnemyUnits.Add(this);
        NowHp = MaxHP = StartHP;
    }

}
