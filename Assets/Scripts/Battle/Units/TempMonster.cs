using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TempMonster : UnitBase
{
    private float barrier;
    public int StartHP;
    [Multiline]
    public string IntentDatas;
    public List<MonIntentData> MonIntentDataList;
    public MonIntentData NowIntent;
    public static TempMonster Inst;
    private void Awake()
    {
        Inst = this;
    }

    private new void Start()
    {
        base.Start();
        BattleManager.Inst.EnemyUnits.Add(this);
        NowHp = MaxHP = StartHP;
        MonIntentDataList = new List<MonIntentData>();
    }

    public override float GetBarrier()
    {
        return barrier;
    }

    public override void AddBarrier(float barrier)
    {
        this.barrier += barrier;
    }

    public override void GetDamage(float amount)
    {
        if (barrier > 0)
        {
            barrier -= amount;
            if (barrier < 0)
            {
                NowHp += barrier; // 남은 데미지를 HP에 적용
                barrier = 0;
            }
        }
        else
        {
            NowHp -= amount;
        }

        if (NowHp < 0) NowHp = 0;

        Debug.Log($"Monster took damage. Barrier: {barrier}, HP: {NowHp}");
    }

    public void ParseIntent()
    {
        string[] lines = IntentDatas.Split('\n');

        foreach (string line in lines)
        {
            string[] parts = line.Split('\t');
            if (parts.Length != 3) continue;

            float probability = float.Parse(parts[0]);
            int id = int.Parse(parts[1]);
            int parameter = int.Parse(parts[2]);

            CardEffectData effectData = DataParser.Inst.GetCardEffectFromListByIndex(id);
            effectData.Amount = parameter;

            MonIntentData monIntentData = new MonIntentData
            {
                Probability = probability,
                CardEffectData = effectData
            };

            MonIntentDataList.Add(monIntentData);
        }

        ChooseIntent();
    }

    public MonIntentData ChooseIntent()
    {
        float totalProbability = MonIntentDataList.Sum(intent => intent.Probability);
        float randomValue = Random.value * totalProbability;
        float cumulativeProbability = 0f;

        foreach (var intent in MonIntentDataList)
        {
            cumulativeProbability += intent.Probability;
            if (randomValue <= cumulativeProbability)
            {
                NowIntent = intent; 
                Debug.Log($"이번턴 에는 {intent.CardEffectData.InfoString}을 {intent.CardEffectData.Amount}만큼 사용");
                return intent;
            }
        }

        return null; // Should not reach here if probabilities are correctly assigned
    }
}

[System.Serializable]
public class MonIntentData
{
    public float Probability;
    public CardEffectData CardEffectData;
}
