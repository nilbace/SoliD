using CombatMechanism;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static Define;

namespace Managers
{
    public interface IClearAfterBattle { void Clear(); }
}

/// <summary>
/// 카드나 몬스터 패턴의 행동들을 정리해둔 NameSpace입니다
/// 각 행동들을 클래스로 캡슐화하고 담아두어 순서대로 실행합니다.
/// </summary>
namespace CombatMechanism
{
    public enum TargetType { 
        TargetEnemy,
        AnyEnemy, 
        AllEnemies,
        Self,
        Allies
    }
    public abstract class NeedTarget
    {
        public TargetType TargetType;

        public abstract void EffectToTarget(List<Char> Targets);
           }

    #region concreteBehavior
    /// <summary>
    /// 적의 HP를 감소시킵니다. 생성할 때 몇의 Damage를 줄지 결정합니다.
    /// </summary>
    public class AttackTargetedOne : NeedTarget
    {
        public Char Target;
        public float Damage;
        public AttackTargetedOne(float Damage)
        { this.Damage = Damage; TargetType = TargetType.TargetEnemy; }

        public override void EffectToTarget(List<Char> Targets)
        {
            foreach (var Char in Targets)
            {
                Char.NowHP -= Damage;
            }
        }
    }

    public class AttackAllEnemies : NeedTarget
    {
        public override void EffectToTarget(List<Char> Targets)
        {
            throw new System.NotImplementedException();
        }
    }

    #endregion

    #region Interval, ETC...
    public class Interval
    {
        public float time;
        public Interval(float time) { this.time = time; }
        public async Task ApplyInterval()
        {
            await Task.Delay(Mathf.CeilToInt(time * 1000));
        }
    }
    #endregion

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
                await ExecuteSteps(mechanism);
            }
        }

        async Task ExecuteSteps(object mechanism)
        {
            if (mechanism is IToTargetEnemy TargetMonster)
            {
                TargetMonster.EffectToTarget(Target);
            }
            else if(mechanism is IToAllEnemies AllMonsters)
            {
                //AllMonsters.
            }
            else if (mechanism is Interval delayMechanism)
            {
                await delayMechanism.ApplyInterval();
            }
        }
    }

}
