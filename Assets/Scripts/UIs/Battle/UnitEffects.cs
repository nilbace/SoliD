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
        // ���� �ڽ� ������Ʈ�� ��� ����
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // ActiveEffects ����Ʈ�� �� ȿ���� ���� ������ ���� �� ����
        for (int i = 0; i < thisUnit.ActiveEffectList.Count; i++)
        {
            GameObject newIcon = Instantiate(Icon, transform);
            newIcon.GetComponent<EffectIcon>().SetIcon(thisUnit.ActiveEffectList[i]);
        }
    }
}
