using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager
{
    public List<UnitBase> Units;
    public UnitBase TargetMonster;
    public List<UnitBase> GetProperUnits(UnitBase unit, E_TargetType targetType)
    {
        List<UnitBase> tempUnits = new List<UnitBase>();
        switch (targetType)
        {
            case E_TargetType.TargetEnemy:
                tempUnits.Add(TargetMonster);
                break;
            case E_TargetType.AllEnemies:
                foreach(UnitBase _unit in Units)
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
}
