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

    private List<GameObject> cards = new List<GameObject>();

    [ContextMenu("ī�� �߰�")]
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

    public void ArrangeCards()
    {
        int totalCards = cards.Count;
        // ��ġ�� �����ϴ� ���� ����
        float startAngle = Mathf.Deg2Rad * -AngleOffset;
        float endAngle = Mathf.Deg2Rad * AngleOffset; 

        // ������ ����� �ʿ��ϴٸ� ���⼭ �߰��� �� �ֽ��ϴ�. ���������� ����ȭ�� ���� ����.
        // ���� ���, �ձ۰� ��ġ�� ���� ��ġ �������� ������ �� �ֽ��ϴ�.
        radius = 30.0f; // ���Ƿ� ������ ������ ��

        for (int i = 0; i < totalCards; i++)
        {
            float t = totalCards > 1 ? i / (float)(totalCards - 1) : 0.5f;
            float angle = Mathf.Lerp(startAngle, endAngle, t);

            // ���� ��ġ ���� x, y ��ġ ��� (z�� ������� �ʰų� �ٸ� �뵵�� ��� ����)
            Vector3 cardPosition = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle) -0.8f, 0.01f * i) * radius;

            // ��ġ�� �߽� ��ġ�� ���� ������ �ʿ��ϴٸ� ���⼭ centerPosition�� ���մϴ�.
            Vector3 targetPosition = centerPosition.position + cardPosition;
            cards[i].transform.DOMove(targetPosition, MoveDuration);

            // ī�尡 ��ġ�� ���� �ùٸ� ������ ����Ű���� z�� ȸ�� ����
            float targetRotation = -(angle * Mathf.Rad2Deg);
            cards[i].transform.DORotate(new Vector3(0, 0, targetRotation), MoveDuration);

            // ��������Ʈ �������� ĵ���� ���� ���� ����
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
