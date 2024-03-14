using System.Collections.Generic;
using System.Threading.Tasks;
using CombatMechanism;
using UnityEngine;
using static Define;

namespace CombatMechanism
{ 
    /// <summary>
    /// ������ ������ ȿ���� �����մϴ�.
    /// </summary>
    public interface IToTargetEnemy { void ToTarget(Char Target); }

    /// <summary>
    /// ��� ������ ���� ȿ���� �����մϴ�.
    /// </summary>
    public interface IToAllEnemies { void ToAllEnemies(List<Char> Target); }
    /// <summary>
    /// ī�带 ����� �������� ȿ���� �����մϴ�
    /// </summary>
    public interface IToSelf { void ToTarget(Char User); }
    /// <summary>
    /// �ൿ ������ ������ ����մϴ�
    /// ex)4��Ÿ ���� ����
    /// </summary>
    public interface IEffectInterval { Task ApplyInterval(); }

    /// <summary>
    /// ���� HP�� ���ҽ�ŵ�ϴ�. ������ �� ���� Damage�� ���� �����մϴ�.
    /// </summary>
    public class AttackTargetedOne : IToTargetEnemy
    {
        public Char Target;
        public float Damage;
        public AttackTargetedOne(float Damage)
        { this.Damage = Damage; }

        public void ToTarget(Char Target)
        {
            Target.NowHP -= Damage;
        }
    }

    public class AttackAllEnemies : IToAllEnemies
    {
        public void ToAllEnemies(List<Char> Target)
        {
            throw new System.NotImplementedException();
        }
    }


    public class Interval : IEffectInterval
    {
        public float time;
        public Interval(float time) { this.time = time; }
        public async Task ApplyInterval()
        {
            await Task.Delay(Mathf.CeilToInt(time * 1000));
        }
    }


}

public static class Define
{
    public enum EffectType { Slash, Sting, MaxCount}
   public abstract class Char
   {
        public string Name { get; set; }
        public float MaxHP { get; set; }
        float _nowHP;
        public float NowHP
        {
            get { return _nowHP; }
            set
            {
                _nowHP = value;
                Debug.Log($"���� HP: {_nowHP}");
            }
        }
        public float ShieldAmount { get; set; }
        public List<Buff> BuffList { get; set; }
    }

    public abstract class PlayableChar : Char
    { 
        public int MaxEnergy { get; set; }
        public int NowEnergy { get; set; }
        public List<Card> Deck { get; set; }

        public PlayableChar() { Deck = new List<Card>(); }
    }


    public class Buff
    {

    }

    public class Card 
    {
        public List<object> MechanismList { get; set; }
        public string CardName { get; set; }
        public int CardCost { get; set; }
        public bool NeedToSpecifyTarget;
        public Char Target;
        public Char User;
        public Sprite CardSpriteIMG;
        public string CardSpriteNameString;
        public Card()
        { MechanismList = new List<object>(); }
         
        public async void UseCard()
        {
            foreach (var mechanism in MechanismList)
            {
                await ExecuteOneStep(mechanism);
            }
        }

        async Task ExecuteOneStep(object mechanism)
        {
            if (mechanism is IToTargetEnemy TargetMonster)
            {
                TargetMonster.ToTarget(Target);
            }
            else if (mechanism is IEffectInterval delayMechanism)
            {
                await delayMechanism.ApplyInterval();
            }
        }
    }

}
