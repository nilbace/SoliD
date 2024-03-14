using System.Collections.Generic;
using System.Threading.Tasks;
using CombatMechanism;
using UnityEngine;
using static Define;

namespace CombatMechanism
{ 
    /// <summary>
    /// 지정된 적에게 효과를 적용합니다.
    /// </summary>
    public interface IToTargetEnemy { void ToTarget(Char Target); }

    /// <summary>
    /// 모든 적에게 다음 효과를 적용합니다.
    /// </summary>
    public interface IToAllEnemies { void ToAllEnemies(List<Char> Target); }
    /// <summary>
    /// 카드를 사용한 유저에게 효과를 적용합니다
    /// </summary>
    public interface IToSelf { void ToTarget(Char User); }
    /// <summary>
    /// 행동 사이의 간격을 명시합니다
    /// ex)4연타 사이 간격
    /// </summary>
    public interface IEffectInterval { Task ApplyInterval(); }

    /// <summary>
    /// 적의 HP를 감소시킵니다. 생성할 때 몇의 Damage를 줄지 결정합니다.
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
                Debug.Log($"현재 HP: {_nowHP}");
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
