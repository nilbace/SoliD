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
    public float radius = 30.0f; // ���Ƿ� ������ ������ ��

    [Header("ī�� 1,2,3���� �� ��ġ")]
    [SerializeField] private float _tValueForOneCard;
    [SerializeField] private Vector2 _tValueForTwoCards;
    [SerializeField] private Vector3 _tValueForThreeCards;

    private List<GameObject> cardsInMyHand = new List<GameObject>();
    public DeckManager TempDeck;
    private int cardIndex;

    [ContextMenu("ī�� �߰�")]
    public void AddCardToHand()
    {
        if (TempDeck.Cards.Count == 0) return;
        GameObject newCard = Instantiate(cardPrefab);
        var newcardData = newCard.GetComponent<CardGO>();
        newcardData.thisCardData = TempDeck.Cards[cardIndex++];
        newcardData.SetCardSprite();
        cardsInMyHand.Add(newCard);
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
        if (index >= 0 && index < cardsInMyHand.Count)
        {
            GameObject discardedCard = cardsInMyHand[index];
            cardsInMyHand.RemoveAt(index);
            Destroy(discardedCard);
            ArrangeCards();
        }
    }

    public void DiscardCardFromHand(GameObject cardGO)
    {
        if(cardsInMyHand.Contains(cardGO))
        {
            cardsInMyHand.Remove(cardGO);
            ArrangeCards();
        }
    }

    /// <summary>
    /// ī�尡 3�� �̸��϶��� ������ ��ġ ó���� �Լ�
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    private float GetProperT_Value(int i)
    {
        if (cardsInMyHand.Count == 1)
        {
            return _tValueForOneCard;
        }
        else if (cardsInMyHand.Count == 2)
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
        else if (cardsInMyHand.Count == 3)
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
            // ���� ó��: 1, 2, 3���� �ƴ� ����� �⺻�� ��ȯ
            return 0.5f;
        }
    }


    public void ArrangeCards()
    {
        int totalCards = cardsInMyHand.Count;
        // ��ġ�� �����ϴ� ���� ����
        float startAngle = Mathf.Deg2Rad * -AngleOffset;
        float endAngle = Mathf.Deg2Rad * AngleOffset; 

        // ������ ����� �ʿ��ϴٸ� ���⼭ �߰��� �� �ֽ��ϴ�. ���������� ����ȭ�� ���� ����.
        // ���� ���, �ձ۰� ��ġ�� ���� ��ġ �������� ������ �� �ֽ��ϴ�.
        radius = 30.0f; // ���Ƿ� ������ ������ ��

        for (int i = 0; i < totalCards; i++)
        {
            float t = totalCards > 3 ? i / (float)(totalCards - 1) : GetProperT_Value(i);
            float angle = Mathf.Lerp(startAngle, endAngle, t);

            // ���� ��ġ ���� x, y ��ġ ��� (z�� ������� �ʰų� �ٸ� �뵵�� ��� ����)
            Vector3 cardPosition = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle) -0.8f, 0.01f * i) * radius;

            // ��ġ�� �߽� ��ġ�� ���� ������ �ʿ��ϴٸ� ���⼭ centerPosition�� ���մϴ�.
            Vector3 targetPosition = centerPosition.position + cardPosition;
            cardsInMyHand[i].transform.DOMove(targetPosition, MoveDuration);

            // ī�尡 ��ġ�� ���� �ùٸ� ������ ����Ű���� z�� ȸ�� ����
            float targetRotation = -(angle * Mathf.Rad2Deg);
            cardsInMyHand[i].transform.DORotate(new Vector3(0, 0, targetRotation), MoveDuration);

            // ��������Ʈ �������� ĵ���� ���� ���� ����
            SpriteRenderer[] spriteRenderers = cardsInMyHand[i].GetComponentsInChildren<SpriteRenderer>();
            for (int j = 0; j < spriteRenderers.Length; j++)
            {
                spriteRenderers[j].sortingOrder = startingSortingOrder + i * sortingOrderIncrement - j;
            }

            Canvas[] canvases = cardsInMyHand[i].GetComponentsInChildren<Canvas>();
            foreach (Canvas canvas in canvases)
            {
                canvas.sortingOrder = startingSortingOrder + i * sortingOrderIncrement + 1;
            }
        }
    }
}
