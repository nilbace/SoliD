using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitEffects : MonoBehaviour
{
    private UnitBase thisUnit;
    private GameObject Icon;
    void Start()
    {
        thisUnit = transform.parent.parent.GetComponent<UnitBase>();
        Icon = Resources.Load("Prefabs/IMG_EffectIcon") as GameObject;
        thisUnit.EffectUpdateAction += UpdateUI;
    }

    public void UpdateUI()
    {
        // 현재 자식 오브젝트를 모두 삭제
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // ActiveEffects 리스트의 각 효과에 대해 아이콘 생성 및 설정
        for (int i = 0; i < thisUnit.ActiveEffectList.Count; i++)
        {
            GameObject newIcon = Instantiate(Icon, transform);
            newIcon.GetComponent<EffectIcon>().SetIcon(thisUnit.ActiveEffectList[i]);
        }
    }
}
