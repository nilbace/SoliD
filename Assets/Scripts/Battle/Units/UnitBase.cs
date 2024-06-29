using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class UnitBase : MonoBehaviour
{
    public float _nowHp;
    public float MaxHP;
    public bool IsInjured;
    public bool IsChained;
    public List<EffectBase> ActiveEffectList;
    public Action EffectUpdateAction;
    public abstract float GetBarrier();
    public abstract void AddBarrier(float barrier);


    public void Start()
    {
        ActiveEffectList = new List<EffectBase>();
    }

    [ContextMenu("Show Active Effects")]
    private void ShowActiveEffects()
    {
        // ���� Ȱ��ȭ�� ����Ʈ�� ǥ���մϴ�.
        foreach (EffectBase effect in ActiveEffectList)
        {
            Debug.Log($"Effect Type: {effect.Type}, Duration: {effect.Duration}, Stack: {effect.Stack}, InfoText: {effect.InfoText}");
        }
    }

    public void AddEffect(EffectBase effect)
    {
        ActiveEffectList.Add(effect);
        EffectUpdateAction?.Invoke();
    }

    public bool HasEffect(E_EffectType effectType)
    {
        return ActiveEffectList.Any(e => e.Type == effectType);
    }

    public float NowHp
    {
        get { return _nowHp; }
        set
        {
            float clampedValue = Mathf.Clamp(value, -100f, MaxHP);

            //HP�� ������ ��
            if (clampedValue < _nowHp)
            {
                _nowHp = clampedValue;
                //ó������ HP�� 0���ϰ� �Ǿ��ٸ� Deadȣ��
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

    public abstract void GetDamage(float amount);

    public void Heal(float amount)
    {
        NowHp += amount;
    }

}
