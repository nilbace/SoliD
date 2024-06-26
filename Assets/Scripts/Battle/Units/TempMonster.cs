using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempMonster : UnitBase
{
    public int StartHP;
    private void Start()
    {
        BattleManager.Inst.EnemyUnits.Add(this);
        NowHp = MaxHP = StartHP;
    }

}
