using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EffectIcon : MonoBehaviour
{
    private Image IMG_icon;
    private TMP_Text TMP_Count;

    public void SetIcon(EffectBase effect)
    {
        if (TryGetComponent(out IMG_icon))
            TMP_Count = GetComponentInChildren<TMP_Text>();
        IMG_icon.sprite = GameManager.Battle.GetEffectIcon(effect.Type);
        TMP_Count.text = ((effect.Duration != -1) ? effect.Duration : effect.Stack).ToString();
    }
}
