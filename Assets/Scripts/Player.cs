using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
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
        tempJaeyoung.Deck.Add(card1);
        tempJaeyoung.Deck[0].UseCard();
    }

    void Update()
    {
        
    }
}
