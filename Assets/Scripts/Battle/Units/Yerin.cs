using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yerin : UnitBase
{
    private void Start()
    {
        BattleManager.Inst.PlayerUnits.Add(this); BattleManager.Inst.ArrangePlayerChars();
    }
}
