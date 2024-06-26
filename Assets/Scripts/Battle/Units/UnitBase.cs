using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitBase : MonoBehaviour
{
    protected float _nowHp;
    public float MaxHP;
    public bool IsInjured;
    public bool IsChained;
    public List<EffectBase> ActiveEffects = new List<EffectBase>();
    
    public bool HasEffect(E_CardEffectType effectType)
    {
        return ActiveEffects.Any(e => e.Type == effectType);
    }

    public float NowHp
    {
        get { return _nowHp; }
        set
        {
            float clampedValue = Mathf.Clamp(value, -100f, MaxHP);

            //HP가 감소할 때
            if (clampedValue < _nowHp)
            {
                _nowHp = clampedValue;
                //처음으로 HP가 0이하가 되었다면 Dead호출
                if (_nowHp <= 0) Dead();
            }
            else
            {
                _nowHp = clampedValue;
            }
        }
    }

    public float DamagedAmount()
    {
        return MaxHP - NowHp;
    }
    public bool isAlive()
    {
        return NowHp > 0;
    }

    public virtual void Dead() { }

    public void GetDamage(float amount)
    {
        NowHp -= amount;
    }

    public void Heal(float amount)
    {
        NowHp += amount;
    }

}
