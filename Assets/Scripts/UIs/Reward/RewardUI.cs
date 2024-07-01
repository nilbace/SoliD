using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardUI : MonoBehaviour
{
    public static RewardUI Inst;
    public GameObject GoldRewardGO;
    public Transform ParentTr;

    private void Awake()
    {
        Inst = this;
    }

    [ContextMenu("°ñµå »ý¼º")]
    public void InstantiateGoldReward()
    {
        GameObject go = Instantiate(GoldRewardGO, ParentTr);
        GoldReward.GoldRewardAmount = 100;
    }

    public void AddGold()
    {
        StartCoroutine(AddGoldOverTime(1f));
    }

    private IEnumerator AddGoldOverTime(float duration)
    {
        int initialGold = GameManager.UserData.NowGold;
        int targetGold = initialGold + GoldReward.GoldRewardAmount;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            int currentGold = Mathf.RoundToInt(Mathf.Lerp(initialGold, targetGold, elapsedTime / duration));
            GameManager.UserData.AddGold(currentGold - GameManager.UserData.NowGold);

            yield return null;
        }

        // Ensure the final value is set correctly
        GameManager.UserData.AddGold(targetGold - GameManager.UserData.NowGold);
    }
}
