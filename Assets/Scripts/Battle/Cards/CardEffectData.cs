using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_TargetType { None,TargetEnemy, AllEnemies, Self}
public enum E_CardEffectType
{ Damage, Encroachment, Interval, Shield, Strength, Thorn, Bloodstain, Concussion, Chain, Heal,
    Black, Crystallization, Blade, Blessing, Despair, MuscleLoss, BulletMark, Vulnerability }

[System.Serializable]
public class CardEffectData
{
    public int EffectID;
    public E_TargetType TargetType;
    public E_CardEffectType CardEffectType;
    public int Amount;
}
