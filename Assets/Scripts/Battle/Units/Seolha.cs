using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seolha : UnitBase
{
    private new void Start()
    {
        base.Start();
        BattleManager.Inst.PlayerUnits.Add(this);
        BattleManager.Inst.ArrangePlayerChars();
    }
}
