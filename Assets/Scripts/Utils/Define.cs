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
/// ī�峪 ���� ������ �ൿ���� �����ص� NameSpace�Դϴ�
/// �� �ൿ���� Ŭ������ ĸ��ȭ�ϰ� ��Ƶξ� ������� �����մϴ�.
/// </summary>
namespace CombatMechanism
{
    public enum TargetType { 
        TargetEnemy,
        AnyEnemy, 
        AllEnemies,
        Self,
        AnyAlly,
        AllAllies,
        MaxCount
    }

    public enum EffectType
    {
        Slash,
        Burn,
        Ice,
        MaxCount
    }

    [System.Serializable]
    public class MechanismData
    {
        public int EffectID;
        public TargetType TargetType;
        public EffectType EffectType;
        public float Interval;
        //public Buff Buff;
        public int Damage;
        public int Shield;
    }
}

public static class Define
{
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
                await ExecuteSteps(mechanism);
            }
        }

        async Task ExecuteSteps(object mechanism)
        {
            
        }
    }

}
