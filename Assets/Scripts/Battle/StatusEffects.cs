using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public abstract class EffectBase
{
    public E_CardEffectType Type { get; private set; }
    /// <summary>
    /// -1�̶�� ���ӽð��� ���� Ÿ��
    /// </summary>
    public int Duration;
    public int Stack;
    public string InfoText;

    public EffectBase(E_CardEffectType effectType, int duration, int stack, string infoText)
    {
        Type = effectType;
        Duration = duration;
        Stack = stack;
        InfoText = infoText;
    }

    public abstract void ApplyEffect(UnitBase unit);

    public virtual void NextTurnStarted(UnitBase unit)
    {
        if (Duration > 0) Duration--;
        if (Duration == 0) unit.ActiveEffects.Remove(this);
    }
    protected virtual void ApplyOrUpdateEffectByStack(UnitBase unit)
    {
        var existingEffect = unit.ActiveEffects.FirstOrDefault(e => e.Type == this.Type);

        if (existingEffect != null)
        {
            existingEffect.Stack += this.Stack;
            if (existingEffect.Stack == 0)
            {
                unit.ActiveEffects.Remove(existingEffect);
            }
        }
        else
        {
            unit.ActiveEffects.Add(this);
        }
    }

    protected virtual void ApplyOrUpdateEffectByDuration(UnitBase unit)
    {
        var existingEffect = unit.ActiveEffects.FirstOrDefault(e => e.Type == this.Type);

        if (existingEffect != null)
        {
            existingEffect.Duration += this.Duration;
            if (existingEffect.Duration == 0)
            {
                unit.ActiveEffects.Remove(existingEffect);
            }
        }
        else
        {
            unit.ActiveEffects.Add(this);
        }
    }
}

public class Strength : EffectBase
{
    public Strength(int stack) : base(E_CardEffectType.Strength,-1, stack , 
        "������ �ִ� ���ط��� �� ��ġ��ŭ �����Ѵ�. ���� ���� ����") { }

    public override void ApplyEffect(UnitBase unit)
    {
        ApplyOrUpdateEffectByStack(unit);
    }
}

public class Crystallization : EffectBase
{
    public Crystallization(int stack) : base(E_CardEffectType.Crystallization, -1, stack, 
        "�� ȹ�� ��, �ش� ��ġ��ŭ �߰� ���� ����. ���� ���� ����") { }
    public override void ApplyEffect(UnitBase unit)
    {
        ApplyOrUpdateEffectByStack(unit);
    }
}

public class Blessing : EffectBase
{
    public Blessing(int stack) : base(E_CardEffectType.Blessing, -1, stack,
        "ĳ���� ȸ�� ��, �ش� ��ġ��ŭ �߰� ȸ������ ����. ���� ���� ����") { }
    public override void ApplyEffect(UnitBase unit)
    {
        ApplyOrUpdateEffectByStack(unit);
    }
}

public class Vulnerability : EffectBase
{
    public Vulnerability(int duration) : base(E_CardEffectType.Vulnerability, duration, -1,
        "������ ���ظ� ���� �� 50%(�Ҽ��� ����)�� ���ظ� �߰��� �Դ´�. ���� �� ���۽� 1 ����") { }
    public override void ApplyEffect(UnitBase unit)
    {
        ApplyOrUpdateEffectByDuration(unit);
    }
}

public class Weakening : EffectBase
{
    public Weakening(int duration) : base(E_CardEffectType.Weakening, duration, -1,
        "������ �ִ� ���ط��� 25%(�Ҽ��� ����)��ŭ �پ���. ���� �� ���۽� 1 ����") { }
    public override void ApplyEffect(UnitBase unit)
    {
        ApplyOrUpdateEffectByDuration(unit);
    }
}

public class Thorn : EffectBase
{
    public Thorn(int stack) : base(E_CardEffectType.Thorn, -1, stack,
        "������ ���� ���ظ� ������ ���� ��󿡰� ���� ��ġ��ŭ ���ظ� �ش�. ���� ���� ����"){ }
    public override void ApplyEffect(UnitBase unit)
    {
        ApplyOrUpdateEffectByStack(unit);
    }
}

public class Bloodstain : EffectBase
{
    public Bloodstain(int stack) : base(E_CardEffectType.Bloodstain, -1, stack,
        "������ ���� ���ظ� ������ ���� ��󿡰� ���� ��ġ��ŭ ���ظ� �ش�. ���� ���� ����")
    { }
    public override void ApplyEffect(UnitBase unit)
    {
        ApplyOrUpdateEffectByStack(unit);
    }

    public new void NextTurnStarted(UnitBase unit)
    {
        unit.NowHp -= Stack;
        unit.ActiveEffects.Remove(this);
    }
}

public class Chain : EffectBase
{
    public Chain(int duration) : base(E_CardEffectType.Chain, duration, -1,
        "�ش� ��ġ��ŭ�� �� ���� ��� Ȥ�� ȸ�� ī�带 ����� �� ����. ���� �� ���� �� 1 ����")
    { }
    public override void ApplyEffect(UnitBase unit)
    {
        ApplyOrUpdateEffectByDuration(unit);
        unit.IsChained = true;
    }

    public new void NextTurnStarted(UnitBase unit)
    {
        if (Duration > 0) Duration--;
        if (Duration == 0)
        {
            unit.ActiveEffects.Remove(this);
            unit.IsChained = false;
        }
    }
}

public class Encroachment : EffectBase
{
    public Encroachment(int duration) : base(E_CardEffectType.Encroachment, duration, -1,
        "ȸ������ 50%(�Ҽ��� ����)��ŭ �پ���. ���� �� ���� �� 1 ����")
    { }
    public override void ApplyEffect(UnitBase unit)
    {
        ApplyOrUpdateEffectByDuration(unit);
    }

}

public class Blade : EffectBase
{
    public Blade(int stack) : base(E_CardEffectType.Blade, -1, stack,
        "0 �ڽ�Ʈ ī�� ��� �� �ش� ��ġ��ŭ �߰� �������� ���Ѵ�. ���� ���� ����")
    { }

    public override void ApplyEffect(UnitBase unit)
    {
        ApplyOrUpdateEffectByStack(unit);
    }
}

public class BulletMark : EffectBase
{
    public BulletMark(int duration) : base(E_CardEffectType.BulletMark, duration, -1,
        "ȸ������ 50%(�Ҽ��� ����)��ŭ �پ���. ���� �� ���� �� 1 ����")
    { }
    public override void ApplyEffect(UnitBase unit)
    {
        ApplyOrUpdateEffectByDuration(unit);
    }
}

public class Injury : EffectBase
{
    public Injury(int stack) : base(E_CardEffectType.Injury, -1, stack,
        "�ൿ�Ұ�. ���� �� ���� �� 1 ����")
    { }

    public override void ApplyEffect(UnitBase unit)
    {
        ApplyOrUpdateEffectByStack(unit);
    }
}

public class Concussion : EffectBase
{
    public Concussion(int stack) : base(E_CardEffectType.Concussion, -1, stack,
        "3ȸ ��ø�� ��, '�λ�'���� ��ȯ�ȴ�. ��ȯ ���Ŀ��� ��ø�� ���� �������.")
    { }
    public override void ApplyEffect(UnitBase unit)
    {
        ApplyOrUpdateEffectByStack(unit);
    }
}

/// <summary>
/// Todo : BattleManager������ �ٽ� �����
/// </summary>
public class Despair : EffectBase
{
    public Despair(int stack) : base(E_CardEffectType.Despair, -1, stack,
        "�� ���� ��, ��ġ��ŭ ��� ������ ����� ��ġ��ŭ ���Ѵ�. ���� ���� ����")
    { }
    public override void ApplyEffect(UnitBase unit)
    {
        ApplyOrUpdateEffectByStack(unit);
    }

    public new void NextTurnStarted(UnitBase unit)
    {
        //���� ã�Ƽ�
        //List<UnitBase> units = new List<UnitBase>();
        //foreach(UnitBase _unit in units)
        //{
        //    _unit.ActiveEffects.Add(new Encroachment(1));
        //}
    }
}

public class MuscleLoss : EffectBase
{
    public MuscleLoss(int stack) : base(E_CardEffectType.MuscleLoss, -1, stack,
        "�� ���� ��, ��ġ��ŭ ���� �����Ѵ�. ")
    { }
    public override void ApplyEffect(UnitBase unit)
    {
        ApplyOrUpdateEffectByStack(unit);
    }

    public new void NextTurnStarted(UnitBase unit)
    {
        unit.ActiveEffects.Add(new Strength(-1));
    }
}

public class Scabbard : EffectBase
{
    public Scabbard(int stack) : base(E_CardEffectType.Scabbard, -1, stack,
        "�� ���� ��, ��ġ��ŭ ���ظ� �޴´�. ���� �� ���� �� 1 �����Ѵ�. ")
    { }
    public override void ApplyEffect(UnitBase unit)
    {
        ApplyOrUpdateEffectByStack(unit);
    }

    public new void NextTurnStarted(UnitBase unit)
    {
        unit.NowHp--;
    }
}
