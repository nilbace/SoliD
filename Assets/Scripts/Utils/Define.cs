using System.Collections.Generic;
using System.Threading.Tasks;
using CombatMechanism;
using UnityEngine;
using static Define;

namespace CombatMechanism
{ 
    public interface IAttackTarget { void AttackTarget(); }
    public interface IGenerateShield { void GenerateShield(Char Target, float Amount); }
    public interface IApplyBuff { void ApplyBuff(Char Target, Buff Buff); }
    public interface IApplyDelay
    {
        Task ApplyDelay();
    }
    public interface ISpawnEffect { void SpwanEffect(Char Target, EffectType effectType); }
    public interface IChangeEnergy { void ChangeEnergy(Char Target, int Value); }

    public class AttackOneTarget : IAttackTarget
    {
        public Char Target;
        public float Damage;
        public AttackOneTarget(Char Target, float Damage)
        { this.Target = Target; this.Damage = Damage; }

        public void AttackTarget()
        {
            Target.NowHP -= Damage;
        }
    }

    public class Delay : IApplyDelay
    {
        public float time;
        public Delay(float time) { this.time = time; }
        public async Task ApplyDelay()
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
                Debug.Log($"ÇöÀç HP: {_nowHP}");
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
        public Char AttackTarget;
        public Sprite CardSpriteIMG;
        public string CardSpriteNameString;
         
        public async void UseCard()
        {
            foreach (var mechanism in MechanismList)
            {
                await ExecuteOneStep(mechanism);
            }
        }

        async Task ExecuteOneStep(object mechanism)
        {
            if (mechanism is IAttackTarget attackMechanism)
            {
                attackMechanism.AttackTarget();
            }
            else if (mechanism is IApplyDelay delayMechanism)
            {
                await delayMechanism.ApplyDelay();
            }
        }
    }

}
