using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using CombatMechanism;

public class Player : MonoBehaviour
{
    public class Jaeyoung : PlayableChar
    { }
    public class Wolf : Char { }

    Jaeyoung tempJaeyoung = new Jaeyoung();
    Wolf tempwolf = new Wolf();
    void Start()
    {
        tempwolf.NowHP = 70;
        Card card1 = new Card();
        
        card1.Target = tempwolf;
        card1.MechanismList.Add(new AttackTargetedOne(10));
        card1.MechanismList.Add(new Interval(0.7f));
        card1.MechanismList.Add(new AttackTargetedOne(10));
        card1.MechanismList.Add(new Interval(0.7f));
        card1.MechanismList.Add(new AttackTargetedOne(10));
        card1.MechanismList.Add(new Interval(0.7f));
        card1.MechanismList.Add(new AttackTargetedOne(10));

        tempJaeyoung.Deck.Add(card1);
        tempJaeyoung.Deck[0].UseCard();
    }

    void Update()
    {
        
    }
}
