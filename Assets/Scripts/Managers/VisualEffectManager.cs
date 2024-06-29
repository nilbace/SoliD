using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualEffectManager : MonoBehaviour
{
    public static VisualEffectManager Inst;
    public List<GameObject> EffectList;
    private void Awake()
    {
        Inst = this;
    }

    public void InstantiateEffect(E_EffectType effectType, Vector3 poz)
    {
        for (int i = 0; i < EffectList.Count; i++)
        {
            if (EffectList[i].name == effectType.ToString())
            {
                Instantiate(EffectList[i], poz, Quaternion.identity);
                return;
            }
        }
    }

    public void InstantiateEffect(E_EffectType effectType, GameObject Go)
    {
        InstantiateEffect(effectType, Go.transform.position);
    }

    public void InstantiateEffect(E_EffectType effectType, UnitBase Unit)
    {
        InstantiateEffect(effectType, Unit.transform.position);
    }
}
