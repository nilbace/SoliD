using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HandManager : MonoSingleton<HandManager>
{
    public GameObject cardPrefab;
    public Transform bottomPosition;
    public Transform centerPosition;
    public Transform topPosition;
    public float curvature = 1.0f;
    public float cardSpacing = 1.0f;
    public int startingSortingOrder = 0;
    public int sortingOrderIncrement = 10;
    public int AngleOffset;
    public float MoveDuration;
    public float radius = 30.0f; // 임의로 설정한 반지름 값

    [Header("카드 1,2,3장일 떄 위치")]
    [SerializeField] private float _tValueForOneCard;
    [SerializeField] private Vector2 _tValueForTwoCards;
    [SerializeField] private Vector3 _tValueForThreeCards;

    private List<GameObject> cards = new List<GameObject>();

    [ContextMenu("카드 추가")]
    public void AddCardToHand()
    {
        GameObject newCard = Instantiate(cardPrefab);
        cards.Add(newCard);
        ArrangeCards();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.D))
        {
            AddCardToHand();
        }
    }

    public void DiscardCardFromHand(int index)
    {
        if (index >= 0 && index < cards.Count)
        {
            GameObject discardedCard = cards[index];
            cards.RemoveAt(index);
            Destroy(discardedCard);
            ArrangeCards();
        }
    }

    /// <summary>
    /// 카드가 3장 미만일때의 적절한 위치 처리용 함수
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    private float GetProperT_Value(int i)
    {
        if (cards.Count == 1)
        {
            return _tValueForOneCard;
        }
        else if (cards.Count == 2)
        {
            if (i == 0)
            {
                return _tValueForTwoCards.x;
            }
            else
            {
                return _tValueForTwoCards.y;
            }
        }
        else if (cards.Count == 3)
        {
            if (i == 0)
            {
                return _tValueForThreeCards.x;
            }
            else if (i == 1)
            {
                return _tValueForThreeCards.y;
            }
            else
            {
                return _tValueForThreeCards.z;
            }
        }
        else
        {
            // 예외 처리: 1, 2, 3장이 아닌 경우의 기본값 반환
            return 0.5f;
        }
    }


    public void ArrangeCards()
    {
        int totalCards = cards.Count;
        // 아치를 형성하는 각도 범위
        float startAngle = Mathf.Deg2Rad * -AngleOffset;
        float endAngle = Mathf.Deg2Rad * AngleOffset; 

        // 반지름 계산이 필요하다면 여기서 추가할 수 있습니다. 예제에서는 간단화를 위해 생략.
        // 예를 들어, 둥글게 배치할 때의 아치 반지름을 지정할 수 있습니다.
        radius = 30.0f; // 임의로 설정한 반지름 값

        for (int i = 0; i < totalCards; i++)
        {
            float t = totalCards > 3 ? i / (float)(totalCards - 1) : GetProperT_Value(i);
            float angle = Mathf.Lerp(startAngle, endAngle, t);

            // 원형 아치 위의 x, y 위치 계산 (z는 사용하지 않거나 다른 용도로 사용 가능)
            Vector3 cardPosition = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle) -0.8f, 0.01f * i) * radius;

            // 아치의 중심 위치에 대한 조정이 필요하다면 여기서 centerPosition을 더합니다.
            Vector3 targetPosition = centerPosition.position + cardPosition;
            cards[i].transform.DOMove(targetPosition, MoveDuration);

            // 카드가 아치를 따라 올바른 방향을 가리키도록 z축 회전 조정
            float targetRotation = -(angle * Mathf.Rad2Deg);
            cards[i].transform.DORotate(new Vector3(0, 0, targetRotation), MoveDuration);

            // 스프라이트 렌더러와 캔버스 정렬 순서 조정
            SpriteRenderer[] spriteRenderers = cards[i].GetComponentsInChildren<SpriteRenderer>();
            for (int j = 0; j < spriteRenderers.Length; j++)
            {
                spriteRenderers[j].sortingOrder = startingSortingOrder + i * sortingOrderIncrement - j;
            }

            Canvas[] canvases = cards[i].GetComponentsInChildren<Canvas>();
            foreach (Canvas canvas in canvases)
            {
                canvas.sortingOrder = startingSortingOrder + i * sortingOrderIncrement + 1;
            }
        }
    }
}
