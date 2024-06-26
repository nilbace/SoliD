using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitHpBar : MonoBehaviour
{
    public UnitBase UnitParent;
    private Slider HpSlider;
    void Start()
    {
        UnitParent = transform.parent.parent.GetComponent<UnitBase>();
        HpSlider = GetComponent<Slider>();
    }

    void Update()
    {
        HpSlider.value = (float)UnitParent.NowHp / (float)UnitParent.MaxHP;
    }
}
