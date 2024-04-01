using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class Define
{
    public enum CharName
    {
        Minju,
        Seolha,
        Yerin
    }

    public enum CharNameKor
    {
        홍민주,
        최설하,
        황예린,
    }

    public abstract class Char : MonoBehaviour
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

    public enum TargetType
    {
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

    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Inst { get; private set; }
        protected virtual void Awake() => Inst = FindObjectOfType(typeof(T)) as T;
    }

}
